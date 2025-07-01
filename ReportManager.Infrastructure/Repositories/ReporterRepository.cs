using System;
using System.Collections.Generic;
using System.Linq;
using ReportManager.Application.Interfaces;
using ReportManager.Domain.Entities;
using ReportManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ReportManager.Infrastructure.Repositories
{
    /// <summary>
    /// EF Core implementation of IReporterRepository.
    /// </summary>
    public class ReporterRepository : IReporterRepository
    {
        private readonly NewsDbContext _context;

        public ReporterRepository(NewsDbContext context)
        {
            _context = context;
        }

        public Reporter GetById(Guid id)
        {
            return _context.Reporters
                .AsNoTracking()
                .FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Reporter> GetAll()
        {
            return _context.Reporters
                .AsNoTracking()
                .ToList();
        }
    }
}