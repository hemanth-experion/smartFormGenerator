using Autofac;
using Experion.Marina.Business.Services.Contracts;
using Experion.Marina.Dto;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Experion.Marina.Web.Api.Controllers
{
    public class HomeController : ApiController
    {
        private readonly IComponentContext _iocContext;

        public HomeController(IComponentContext iocContext)
        {
            _iocContext = iocContext;
        }


        [HttpPost]
        [Route("login")]
        public ResponseDto<UserValidDto> IsValidUser(UserDto user)
        {
            var userService = _iocContext.Resolve<IUserService>();
            var responseDto = new ResponseDto<UserValidDto>();

            try
            {
                var validUser = userService.IsValidUser(user);
                responseDto.Data = validUser ?? null;
                responseDto.Messages = null;
            }
            catch (Exception)
            {
                responseDto = null;
                throw;
            }
            return responseDto;
        }


        //Insert Template to Database
        [HttpPost]
        [Route("createTemplate")]
        public ResponseDto<UserValidDto> CreateTemplate(TemplateDTO templateDto)
        {
            var userService = _iocContext.Resolve<ITemplateService>();
            
            var responseDto = new ResponseDto<UserValidDto>();

            try
            {
                var validUser = userService.CreateTemplate(templateDto);
                responseDto.Data = validUser ?? null;
                responseDto.Messages = null;
            }
            catch (Exception)
            {
                responseDto = null;
                throw;
            }
            return responseDto;

        }

        //Get Template Name of specified Template Id
        [HttpGet]
        [Route("templateName/{userId}")]
        public ResponseDto<IEnumerable<TemplateNameDTO>> GetTemplateName(long userId)
        {
            var sampleService = _iocContext.Resolve<ITemplateService>();
            var responseDto = new ResponseDto<IEnumerable<TemplateNameDTO>>();

            try
            {
                var sample = sampleService.GetTemplateName(userId);
                responseDto.Data = sample ?? null;
                responseDto.Messages = null;
            }
            catch (Exception)
            {
                responseDto = null;
                throw;
            }
            return responseDto;
        }

        //Insert Template Name to Database
        [HttpPost]
        [Route("addTemplateName")]
        public long SetTemplateName(TemplateNameDTO templateDto)
        {
            var userService = _iocContext.Resolve<ITemplateService>();            
            return userService.SetTemplateName(templateDto);
        }

        //Get Template with specified Id
        [HttpGet]
        [Route("template/{templateId}")]
        public ResponseDto<IEnumerable<TemplateDTO>> ViewTemplate(long templateId)
        {
            var sampleService = _iocContext.Resolve<ITemplateService>();
            var responseDto = new ResponseDto<IEnumerable<TemplateDTO>>();

            try
            {
                var sample = sampleService.GetTemplate(templateId);
                responseDto.Data = sample ?? null;
                responseDto.Messages = null;
            }
            catch (Exception)
            {
                responseDto = null;
                throw;
            }
            return responseDto;
        }

    }
}
