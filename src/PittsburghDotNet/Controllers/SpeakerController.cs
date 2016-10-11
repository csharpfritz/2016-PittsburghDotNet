using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PittsburghDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PittsburghDotNet.Controllers
{

  public class SpeakerController : Controller
  {

    public SpeakerController(SpeakerContext context)
    {
      this.Context = context;
    }

    public SpeakerContext Context { get; }

    public async Task<IActionResult> Index()
    {

      var speakers = await Context.Speakers.ToListAsync();
      return View(speakers);

    }

  }

}
