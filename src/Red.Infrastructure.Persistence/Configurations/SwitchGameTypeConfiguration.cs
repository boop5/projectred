using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Red.Core.Application.Json;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Persistence.Configurations
{
    internal sealed class SwitchGameTypeConfiguration : IEntityTypeConfiguration<SwitchGame>
    {
        public void Configure(EntityTypeBuilder<SwitchGame> builder)
        {
            var serializerOptions = AppJsonOptions.Default;

            // todo: use pk on fsid only
            builder.HasKey(x => x.FsId)
                   .HasName("PK_SwitchGame_FsID"); 
            
            // todo: add index for website filters (search by category, ..)

            builder.Property(x => x.Media)
                   .HasConversion(
                       x => JsonSerializer.Serialize(x, serializerOptions),
                       x => JsonSerializer.Deserialize<CountryDictionary<SwitchGameMedia>>(x, serializerOptions) ?? new())
                   .HasDefaultValue(new CountryDictionary<SwitchGameMedia>());
   
            builder.Property(x => x.Languages)
                   .HasConversion(
                       x => JsonSerializer.Serialize(x, serializerOptions),
                       x => JsonSerializer.Deserialize<List<string>>(x, serializerOptions) ?? new List<string>());
            builder.Property(x => x.Languages)
                   .Metadata
                   .SetValueComparer(BuildValueComparer<string>());

            builder.Property(x => x.PlayModes)
                   .HasConversion(
                       x => JsonSerializer.Serialize(x, serializerOptions),
                       x => JsonSerializer.Deserialize<SwitchGamePlayModes>(x, serializerOptions)!);

            builder.Property(x => x.Nsuids)
                   .HasConversion(
                       x => JsonSerializer.Serialize(x, serializerOptions),
                       x => JsonSerializer.Deserialize<List<string>>(x, serializerOptions) ?? new List<string>());
            builder.Property(x => x.Nsuids)
                   .Metadata
                   .SetValueComparer(BuildValueComparer<string>());

            builder.Property(x => x.Price)
                   .HasConversion(
                       x => JsonSerializer.Serialize(x, serializerOptions),
                       x => JsonSerializer.Deserialize<CountryDictionary<SwitchGamePriceDetails>>(x, serializerOptions) ?? new CountryDictionary<SwitchGamePriceDetails>());

            builder.Property(x => x.Colors)
                   .HasConversion(
                       x => JsonSerializer.Serialize(x.Select(y => y.HexCode), serializerOptions),
                       x => DeserializeColors(x, serializerOptions));
            builder.Property(x => x.Colors)
                   .Metadata
                   .SetValueComparer(BuildValueComparer<HexColor>());

            builder.Property(x => x.ContentRating)
                   .HasConversion(
                       x => JsonSerializer.Serialize(x, serializerOptions),
                       x => JsonSerializer.Deserialize<CountryDictionary<ContentRating>>(x, serializerOptions) ?? new CountryDictionary<ContentRating>())
                   .HasDefaultValue(new CountryDictionary<ContentRating>());

            builder.Property(x => x.Description)
                   .HasConversion(
                       x => JsonSerializer.Serialize(x, serializerOptions),
                       x => JsonSerializer.Deserialize<CountryDictionary<string>>(x, serializerOptions) ?? new CountryDictionary<string>())
                   .HasDefaultValue(new CountryDictionary<string>());

            builder.Property(x => x.EshopUrl)
                   .HasConversion(
                       x => JsonSerializer.Serialize(x, serializerOptions),
                       x => JsonSerializer.Deserialize<CountryDictionary<string>>(x, serializerOptions) ?? new CountryDictionary<string>())
                   .HasDefaultValue(new CountryDictionary<string>());

            builder.Property(x => x.Popularity)
                   .HasConversion(
                       x => JsonSerializer.Serialize(x, serializerOptions),
                       x => JsonSerializer.Deserialize<CountryDictionary<int>>(x, serializerOptions) ?? new CountryDictionary<int>())
                   .HasDefaultValue(new CountryDictionary<int>());

            builder.Property(x => x.Title)
                   .HasConversion(
                       x => JsonSerializer.Serialize(x, serializerOptions),
                       x => JsonSerializer.Deserialize<CountryDictionary<string>>(x, serializerOptions) ?? new CountryDictionary<string>())
                   .HasDefaultValue(new CountryDictionary<string>());

            builder.Property(x => x.Slug)
                   .HasConversion(
                       x => JsonSerializer.Serialize(x, serializerOptions),
                       x => JsonSerializer.Deserialize<CountryDictionary<string>>(x, serializerOptions) ?? new CountryDictionary<string>())
                   .HasDefaultValue(new CountryDictionary<string>());

            builder.Property(x => x.ProductCode)
                   .HasConversion(
                       x => JsonSerializer.Serialize(x, serializerOptions),
                       x => JsonSerializer.Deserialize<CountryDictionary<string>>(x, serializerOptions) ?? new CountryDictionary<string>())
                   .HasDefaultValue(new CountryDictionary<string>());

            builder.Property(x => x.Categories)
                   .HasConversion(
                       x => JsonSerializer.Serialize(x, serializerOptions),
                       x => JsonSerializer.Deserialize<CountryDictionary<List<string>>>(x, serializerOptions) ?? new CountryDictionary<List<string>>())
                   .HasDefaultValue(new CountryDictionary<List<string>>());
        }

        private static List<HexColor> DeserializeColors(string json, JsonSerializerOptions serializerOptions)
        {
            var deserialized = JsonSerializer.Deserialize<List<string>?>(json, serializerOptions);

            if (deserialized == null)
            {
                return new List<HexColor>();
            }

            var colors = deserialized.Select(x => new HexColor(x)).ToList();
            return colors;
        }

        private static ValueComparer<List<T>?> BuildValueComparer<T>()
        {
            return new(
                (x,y) => x != null && y != null && x.SequenceEqual(y),
                x => x == null ? 0 : x.Aggregate(0, (a,v) => HashCode.Combine(a, v == null ? 0 : v.GetHashCode())),
                x => x == null ? null : x.ToList());
        }
    }
}