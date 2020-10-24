using Microsoft.EntityFrameworkCore;
using SparePartsMagazine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SparePartsMagazine.Services
{
    public class PartsService
    {
        private readonly ApplicationDbContext _context;
        public PartsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Part> GetById(int id)
        {
            return await _context.Parts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Part>> GetAll()
        {
            return await _context.Parts.ToListAsync();
        }

        public async Task<List<Part>> GetAllFiltered(string sortBy,string nameSearch, string priceSearch)
        {
            var parts =  _context.Parts.AsQueryable();
            if (!String.IsNullOrEmpty(nameSearch))
            {
                parts = parts.Where(s => s.Name.Contains(nameSearch));
            }
            if (!String.IsNullOrEmpty(priceSearch))
            {
                priceSearch = priceSearch.Replace(',', '.');
                parts = parts.Where(s => s.Price.ToString().Contains(priceSearch));
            }
            if (string.IsNullOrEmpty(sortBy))
            {
                sortBy = "Name";
            }

            bool descending = false;
            if (sortBy.EndsWith("_desc"))
            {
                sortBy = sortBy.Substring(0, sortBy.Length - 5);
                descending = true;
            }

            if (descending)
            {
                parts = parts.OrderByDescending(e => EF.Property<object>(e, sortBy));
            }
            else
            {
                parts = parts.OrderBy(e => EF.Property<object>(e, sortBy));
            }
            return await parts.ToListAsync();
        }

        public async Task<Part> CreateAsync(Part part)
        {
            try
            {
                part.InputDate = DateTimeOffset.Now;
                part.ModificationDate = DateTimeOffset.Now;
                _context.Parts.Add(part);

                await _context.SaveChangesAsync();

                return part;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to create model", e);
            }
        }

        public async Task<List<Part>> UpdateAsync(int? id,Part updatedPart) 
        {
            try
            {
                updatedPart.ModificationDate = DateTimeOffset.Now;
                _context.Update(updatedPart);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {           
                throw new Exception(e.InnerException.Message);              
            }
            return await _context.Parts.ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var deletedPart = await _context.Parts.FirstOrDefaultAsync(_ => _.Id == id);
                if (deletedPart == null)
                    throw new Exception("Model doesn't exist");

                var deletedEntry = _context.Parts.Remove(deletedPart);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to delete model", e);
            }
        }

    }
}
