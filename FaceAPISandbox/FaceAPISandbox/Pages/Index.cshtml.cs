using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaceAPISandbox.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FaceAPISandbox.Pages
{
    public class IndexModel : PageModel
    {
        private IFaceService _faceService;

        public IndexModel(IFaceService faceService)
        {
            _faceService = faceService;
        }

        public void OnGet()
        {

        }
    }
}
