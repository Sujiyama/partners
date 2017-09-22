using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Uga.Partners.Infrastructure.Cache;
using Nop.Plugin.Uga.Partners.Models;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Security;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Core.Domain.Customers;
using Nop.Services.Common;
using Nop.Core.Data;
using Nop.Core.Domain.Common;
using System.Collections;
using Nop.Plugin.Uga.Partners.Services;
using Nop.Plugin.Uga.Partners.Domain;

namespace Nop.Plugin.Uga.Partners.Controllers
{
    [NopHttpsRequirement(SslRequirement.NoMatter)]
    public class PartnersController : BasePluginController
    {
        #region Fields

      
        private readonly IWorkContext _workContext;
        private readonly IPartnerService _pService;
        private readonly IRepository<PartnersRecord> _pRepository;
        

        #endregion

        #region Ctor

        public PartnersController(IPartnerService pService,
            IWorkContext workContext,
            IRepository<PartnersRecord> pRepository
           
            )
        {
            this._pService = pService;
            this._workContext = workContext;
            this._pRepository = pRepository;
            
        }
        
        #endregion
        
        #region Utilities

        #endregion

        #region Methods

 
        public ActionResult Index()
        {
            return View("~/Plugins/Uga.Partners/Views/Partners/Index.cshtml");
        }


        [HttpPost]
        public ActionResult PartnersGridList(DataSourceRequest request, string id) {
            IList<PartnersModel> partners;
            if (!string.IsNullOrEmpty(id)) {
                 partners = (from c in _pRepository.Table.AsEnumerable()
                                                 where (c.City.ToUpper().Contains(id.ToUpper()) || c.PostCode.ToUpper().Contains(id.ToUpper()))
                                                 select new PartnersModel {
                                                     Id = c.Id, Name = c.Name, Title = c.Description, Email = c.Email,
                                                     Telephone = c.Telephone, PostCode = c.PostCode,
                                                     Address = c.Address, City = c.City
                                                 }).Distinct().ToList();
            }
            else {
                 partners = (from c in _pRepository.Table.AsEnumerable()
                                                 select new PartnersModel {
                                                     Id = c.Id, Name = c.Name, Title = c.Description, Email = c.Email,
                                                     Telephone = c.Telephone, PostCode = c.PostCode,
                                                     Address = c.Address, City = c.City
                                                 }).Distinct().ToList();
            }

            var gridModel = new DataSourceResult {
                Data = partners,
                Total = partners.Count()
            };

            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult PartnersList(string id)
        {
            IList<PartnersModel> partners;
            if (!string.IsNullOrEmpty(id)) {
                partners = (from c in _pRepository.Table.AsEnumerable()
                            where (c.City.ToUpper().Contains(id.ToUpper()) || c.PostCode.ToUpper().Contains(id.ToUpper()))
                            select new PartnersModel {
                                Id = c.Id, Name = c.Name, Title = c.Description, Email = c.Email, Description = c.Description,
                                Telephone = c.Telephone, PostCode = c.PostCode,
                                Address = c.Address, City = c.City, Latitude = c.Latitude, Longtitude = c.Longtitude
                            }).Distinct().ToList();

            }
            else {
                partners = (from c in _pRepository.Table.AsEnumerable()
                            select new PartnersModel {
                                Id = c.Id, Name = c.Name, Title = c.Description, Email = c.Email, Description = c.Description,
                                Telephone = c.Telephone, PostCode = c.PostCode,
                                Address = c.Address, City = c.City, Latitude = c.Latitude, Longtitude = c.Longtitude
                            }).Distinct().ToList();
            }
           

            return Json(partners, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}   