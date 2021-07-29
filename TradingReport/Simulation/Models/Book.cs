using System;
using TradingReport.Simulation.Dtos;

namespace TradingReport.Simulation.Models
{
    class Book
    {
        public string Index { get; init; }

        public string Title { get; init; }

        public double LastModified { get; init; }

        public static Book FromDto(BookDto dto)
        {
            if (!Double.TryParse(dto.LastModified, out double lastModified))
            {
                throw new ArgumentException("invalid lastModified");
            }

            return new Book
            {
                Index = dto.Index,
                Title = dto.Title,
                LastModified = lastModified
            };
        }
    }
}