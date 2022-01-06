using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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
            var objects = _context.CelestialObjects.Where(o => o.Name == name).ToList();
            if (objects.Count == 0) return NotFound();
            foreach (var o in objects)
            {
                o.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == o.Id).ToList();
            }
            return Ok(objects);
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

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject obj)
        {
            _context.CelestialObjects.Add(obj);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = obj.Id }, obj);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CelestialObject updatedObject)
        {
            var obj = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);
            if (obj == null) return NotFound();
            obj.Name = updatedObject.Name;
            obj.OrbitalPeriod = updatedObject.OrbitalPeriod;
            obj.OrbitedObjectId = updatedObject.OrbitedObjectId;
            _context.CelestialObjects.Update(obj);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var obj = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);
            if (obj == null) return NotFound();
            obj.Name = name;
            _context.CelestialObjects.Update(obj);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objects = _context.CelestialObjects.Where(o => o.Id == id || o.OrbitedObjectId == id).ToList();
            if (objects.Count == 0) return NotFound();
            _context.CelestialObjects.RemoveRange(objects);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
