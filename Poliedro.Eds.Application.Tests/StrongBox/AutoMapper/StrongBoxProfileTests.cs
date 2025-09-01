using System;
using AutoMapper;
using Xunit;
using Poliedro.Eds.Application.StrongBox.AutoMapper;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Domain.StrongBox.Entities;

namespace Poliedro.Eds.Application.Tests.StrongBox.AutoMapper
{
    public class StrongBoxProfileTests
    {
        private readonly IMapper _mapper;

        public StrongBoxProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StrongBoxProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Should_Map_StrongBoxEntity_To_StrongBoxDto()
        {
            // Arrange
            var entity = new StrongBoxEntity(
                dateTime: DateTime.Now,
                idCorte: 123,
                type: "CORTE",
                ammount: 100.50m,
                saldo: 500.75m,
                note: "Test note");

            // Act
            var dto = _mapper.Map<StrongBoxDto>(entity);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(entity.DateTime, dto.DateTime);
            Assert.Equal(entity.IdCorte, dto.IdCorte);
            Assert.Equal(entity.Type, dto.Type);
            Assert.Equal(entity.Ammount, dto.Ammount);
            Assert.Equal(entity.Saldo, dto.Saldo);
            Assert.Equal(entity.Note, dto.Note);
        }

        [Fact]
        public void Should_Handle_Null_Values_In_Entity()
        {
            // Arrange
            var entity = new StrongBoxEntity(
                dateTime: DateTime.Now,
                idCorte: null,
                type: "RETIRO",
                ammount: 50.00m,
                saldo: 100.00m,
                note: null);

            // Act
            var dto = _mapper.Map<StrongBoxDto>(entity);

            // Assert
            Assert.NotNull(dto);
            Assert.Null(dto.IdCorte);
            Assert.Null(dto.Note);
            Assert.Equal("RETIRO", dto.Type);
        }

        [Fact]
        public void AutoMapper_Configuration_Should_Be_Valid()
        {
            // Act & Assert
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StrongBoxProfile>();
            });
            
            // This will throw an exception if the configuration is invalid
            config.AssertConfigurationIsValid();
        }
    }
}
