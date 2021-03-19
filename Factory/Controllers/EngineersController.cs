using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Factory.Models;

namespace Factory.Controllers
{
  public class EngineersController : Controller
  {
    private readonly FactoryContext _db;
    public EngineersController(FactoryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Engineers.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "MachineName");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Engineer engineer, int MachineId)
    {
      _db.Engineers.Add(engineer);
      _db.SaveChanges();
      if(MachineId != 0)
      {
        _db.EngineerMachine.Add(new EngineerMachine() {EngineerId = engineer.EngineerId, MachineId = MachineId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Details(int id)
    {
      Engineer thisEngineer = _db.Engineers
        .Include(engineer =>engineer.JoinEntities)
        .ThenInclude(join => join.Machine)
        .FirstOrDefault(engineer =>engineer.EngineerId == id);
      return View(thisEngineer);
    }

    public ActionResult AddMachine(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "MachineName");
      return View(thisEngineer);
    }
    [HttpPost]
    public ActionResult AddMachine(Engineer engineer, int MachineId)
    {
      if(MachineId != 0)
      {
        _db.EngineerMachine.Add(new EngineerMachine() { EngineerId = engineer.EngineerId, MachineId = MachineId});
      }
      _db.SaveChanges();
      return RedirectToAction("Details");
    }
  }
}