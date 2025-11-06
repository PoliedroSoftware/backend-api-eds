using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Court.Commands.CreateCourt;
using Poliedro.Eds.Application.Eds.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Wizard.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Domain.Wizard.DomainSetup;
using Poliedro.Eds.Domain.Wizard.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Wizard.DomainServices.Impl;

public class SetupCreateService(ITenantDbContextFactory dbContextFactory, ILogger<SetupCreateService> logger) : ISetupCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(SetupEntity setupEntity)
    {
        using var context = dbContextFactory.CreateDbContext();

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
                await context.Island.AddAsync(island);
            }

            await context.SaveChangesAsync();

            // create tanks
            foreach (var tank in setupEntity.Tanks)
            {
                await context.Tank.AddAsync(tank);
            }

            await context.SaveChangesAsync();

            // create products
            foreach (var product in setupEntity.Products)
            {
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

            logger.LogInformation("Successfully created Wizard");
return VoidResult.Instance;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return SetupErrorBuilder.SetupCreationException(ex.Message);
        }
    }
}
