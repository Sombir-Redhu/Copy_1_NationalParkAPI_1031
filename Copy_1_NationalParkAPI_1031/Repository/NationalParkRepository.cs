﻿using Copy_1_NationalParkAPI_1031.Data;
using Copy_1_NationalParkAPI_1031.Models;
using Copy_1_NationalParkAPI_1031.Repository.IRepository;

namespace Copy_1_NationalParkAPI_1031.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _context;
        public NationalParkRepository(ApplicationDbContext context)
        {
            _context = context; 
        }

        public bool CreateNationPark(NationalPark nationalPark)
        {
            _context.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationPark(NationalPark nationalPark)
        {
            _context.NationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
          return _context.NationalParks.Find(nationalParkId);          
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _context.NationalParks.ToList();
        }

        public bool NationalParkExists(int nationalParkId)
        {
            return _context.NationalParks.Any(np => np.Id == nationalParkId);
        }

        public bool NationalParkExists(string nationalParkName)
        {
            return _context.NationalParks.Any(np => np.Name == nationalParkName);
        }

        public bool Save()
        {
            return _context.SaveChanges()==1?true:false;
        }

        public bool UpdateNationPark(NationalPark nationalPark)
        {
            _context.NationalParks.Update(nationalPark);
            return Save();  
        }       
    }
}
