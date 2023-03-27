using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Data.Configuration
{
    internal class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasData
            (
                new Person
                {
                    Id = 1,
                    Name = "David",
                    Surname = "Nutter",
                    BirthDate = new DateTime(1960)
                },
                new Person
                {
                    Id = 2,
                    Name = "Alan",
                    Surname = "Taylor",
                    BirthDate = new DateTime(1965)
                },
                new Person
                {
                    Id = 3,
                    Name = "Peter",
                    Surname = "Dinklage",
                    BirthDate = new DateTime(1969)
                }
            );
        }
    }
}
