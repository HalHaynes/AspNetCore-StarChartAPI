using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var obj = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);
            if (obj == null) return NotFound();
            obj.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == id).ToList();
            return Ok(obj);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var obj = _context.CelestialObjects.FirstOrDefault(o => o.Name == name);
            if (obj == null) return NotFound();
            obj.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == o.Id).ToList();
            return Ok(obj);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var objects = _context.CelestialObjects.ToList();
            foreach (var o in objects)
            {
                o.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == o.Id).ToList();
            }
            return Ok(objects);
        }

    }
}
