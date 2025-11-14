using System.Net;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Wizard.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.EdsTank.Entities;
using Poliedro.Eds.Domain.Wizard.DomainSetup;
using Poliedro.Eds.Domain.Wizard.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Wizard.DomainServices.Impl;

public class SetupCreateService(ITenantDbContextFactory dbContextFactory) : ISetupCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(SetupEntity setupEntity)
    {
        using var context = dbContextFactory.CreateDbContext();

        List<string> duplicateMessages = await GetDuplicateMessagesAsync(context, setupEntity);

        if (duplicateMessages.Any())
        {
            return Result<VoidResult, Error>.Failure(
              Error.CreateInstance(
                  code: "SetupAlreadyExists",
                  description: $"{string.Join($"{Environment.NewLine}{Environment.NewLine}", duplicateMessages)}",
                  httpStatusCode: HttpStatusCode.Conflict
              )
          );
        }

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            // create bussiness
            await context.Business.AddAsync(setupEntity.Bussiness);
            await context.SaveChangesAsync();

            var businessId = setupEntity.Bussiness.IdBusiness;

            // create EDS (by businnes)
            foreach (var eds in setupEntity.EDS)
            {
                eds.IdBusiness = businessId;
                await context.Eds.AddAsync(eds);
            }

            await context.SaveChangesAsync();

            // create islands
            foreach (var island in setupEntity.Islands)
            {
                var eds = setupEntity.EDS.FirstOrDefault(t => t.Name == island.NameEDS);

                if (eds != null)
                {
                    island.IdEds = eds.IdEds;
                }

                await context.Island.AddAsync(island);
            }

            await context.SaveChangesAsync();

            // create tanks
            foreach (var tank in setupEntity.Tanks)
            {
                await context.Tank.AddAsync(tank);
            }

            await context.SaveChangesAsync();

            //Create Relation eds-tank
            foreach (var tank in setupEntity.Tanks)
            {
                var eds = setupEntity.EDS.FirstOrDefault(t => t.Name == tank.NameEDS);

                if (eds != null)
                {
                    var edsTank = new EdsTankEntity()
                    {
                        IdEds = eds.IdEds,
                        IdTank = tank.IdTank,
                    };
                    await context.EdsTank.AddAsync(edsTank);
                }
            }

            await context.SaveChangesAsync();

            // create products
            foreach (var product in setupEntity.Products)
            {
                var eds = setupEntity.EDS.FirstOrDefault(t => t.Name == product.NameEDS);

                if (eds != null)
                {
                    product.IdEds = eds.IdEds;
                }

                await context.Product.AddAsync(product);
            }

            await context.SaveChangesAsync();

            // create compartiment(by tank)
            foreach (var compartiment in setupEntity.Compartiments)
            {
                var parentTank = setupEntity.Tanks.FirstOrDefault(t => t.Number == compartiment.NumberTank);

                if (parentTank != null)
                {
                    compartiment.IdTank = parentTank.IdTank;
                }

                var parentProduct = setupEntity.Products.FirstOrDefault(t => t.Name == compartiment.NameProduct);

                if (parentProduct != null)
                {
                    compartiment.IdProduct = parentProduct.IdProduct;
                }

                await context.Compartiment.AddAsync(compartiment);
            }

            await context.SaveChangesAsync();

            // create dispensers(by island)
            foreach (var dispenser in setupEntity.Dispensers)
            {
                var numberIsland = dispenser.NumberIsland ?? 0;
                var island = setupEntity.Islands[numberIsland];

                if (island != null)
                {
                    dispenser.IdIsland = island.IdIsland;
                }

                var eds = setupEntity.EDS.FirstOrDefault(t => t.Name == dispenser.NameEDS);

                if (eds != null)
                {
                    dispenser.EdsId = eds.IdEds;
                }

                await context.Dispensers.AddAsync(dispenser);
            }

            await context.SaveChangesAsync();

            // create hoses(by dispenser)
            foreach (var hose in setupEntity.Hoses)
            {
                var dispenser = setupEntity.Dispensers.FirstOrDefault(d => d.Code == hose.CodeDispenser);

                if (dispenser != null)
                {
                    hose.IdDispensers = dispenser.Id;
                }

                var compartiment = setupEntity.Compartiments.FirstOrDefault(d => d.Number == hose.NumberCompartiment);

                if (compartiment != null)
                {
                    hose.IdCompartiment = compartiment.IdCompartiment;
                }

                await context.Hose.AddAsync(hose);
            }

            await context.SaveChangesAsync();

            // create islanders
            foreach (var islander in setupEntity.Islanders)
            {
                // Validate
                bool exists = await context.Islander
                    .AnyAsync(u => u.Name == islander.Name || u.Email == islander.Email);
                if (exists)
                    throw new Exception($"Usuario islero '{islander.Name}' ya existe.");

                var eds = setupEntity.EDS.FirstOrDefault(t => t.Name == islander.NameEDS);

                if (eds != null)
                {
                    islander.IdEds = eds.IdEds;
                }

                await context.Islander.AddAsync(islander);
            }

            await context.SaveChangesAsync();

            // create providers
            foreach (var provider in setupEntity.Providers)
            {
                await context.Provider.AddAsync(provider);
            }

            await context.SaveChangesAsync();

            // Final commit
            await transaction.CommitAsync();

            return VoidResult.Instance;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return SetupErrorBuilder.SetupCreationException(ex.Message);
        }
    }

    private async Task<List<string>> GetDuplicateMessagesAsync(DataBaseContext context, SetupEntity setupEntity)
    {
        var errors = new List<string>();

        // 1️ Business
        var existsBusiness = await context.Business
            .AnyAsync(b => b.Name.ToLower() == setupEntity.Bussiness.Name.ToLower());
        if (existsBusiness)
            errors.Add("El nombre del negocio ya existe. Ingrese uno diferente por favor.");

        // 2️ EDS
        var edsNames = setupEntity.EDS.Select(b => b.Name.ToLower()).ToList();
        var duplicatedEds = await context.Eds
            .Where(b => edsNames.Contains(b.Name.ToLower()))
            .Select(b => b.Name)
            .ToListAsync();
        if (duplicatedEds.Any())
            errors.Add($"Los siguientes EDS ya existen: {string.Join(", ", duplicatedEds)}");

        // 3️ Islands
        var islandDescriptions = setupEntity.Islands.Select(i => i.Description.ToLower()).ToList();
        var duplicatedIslands = await context.Island
            .Where(i => islandDescriptions.Contains(i.Description.ToLower()))
            .Select(i => i.Description)
            .ToListAsync();
        if (duplicatedIslands.Any())
            errors.Add($"Las siguientes Islas ya existen: {string.Join(", ", duplicatedIslands)}");

        // 4️ Tanks
        var tankNumbers = setupEntity.Tanks.Select(t => t.Number.ToLower()).ToList();
        var duplicatedTanks = await context.Tank
            .Where(t => tankNumbers.Contains(t.Number.ToLower()))
            .Select(t => t.Number)
            .ToListAsync();
        if (duplicatedTanks.Any())
            errors.Add($"Los siguientes Tanques ya existen: {string.Join(", ", duplicatedTanks)}");

        // 5️ Dispensers
        var duplicateDispensers = new List<string>();
        foreach (var d in setupEntity.Dispensers)
        {
            var exists = await context.Dispensers
                .AnyAsync(db => db.Code.ToLower() == d.Code.ToLower() && db.Number == d.Number);
            if (exists)
                duplicateDispensers.Add($"{d.Code}-{d.Number}");
        }
        if (duplicateDispensers.Any())
            errors.Add($"Los siguientes Dispensadores ya existen: {string.Join(", ", duplicateDispensers)}");

        // 6️ Islanders
        var islanderEmails = setupEntity.Islanders.Select(i => i.Email.ToLower()).ToList();
        var islanderNames = setupEntity.Islanders.Select(i => i.Name.ToLower()).ToList();
        var duplicatedIslanders = await context.Islander
            .Where(i => islanderEmails.Contains(i.Email.ToLower()) || islanderNames.Contains(i.Name.ToLower()))
            .Select(i => i.Email)
            .ToListAsync();
        if (duplicatedIslanders.Any())
            errors.Add($"Los siguientes Isleros ya existen(Nombre o Correo): {string.Join(", ", duplicatedIslanders)}");

        // 7 Providers
        var providerNames = setupEntity.Providers.Select(p => p.Name.ToLower()).ToList();
        var duplicatedProviders = await context.Provider
            .Where(p => providerNames.Contains(p.Name.ToLower()))
            .Select(p => p.Name)
            .ToListAsync();
        if (duplicatedProviders.Any())
            errors.Add($"Los siguientes Provedores ya existen: {string.Join(", ", duplicatedProviders)}");

        return errors;
    }

}
