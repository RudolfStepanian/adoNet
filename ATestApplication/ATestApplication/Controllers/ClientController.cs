using System;
using System.Threading.Tasks;
using ATestApplication.BuisenessEntities;
using ATestApplication.Exceptions;
using ATestApplication.Models;
using ATestApplication.Models.Customer;
using ATestApplication.Services.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATestApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : BaseController
    {
        ClientService objClient = new ClientService();

        /// <summary>
        /// Returns all client
        /// </summary>
        [HttpGet, Authorize]
        public async Task<ActionResult<ResponseModel<ClientList>>> Get([FromQuery] ClientFilterModel filter)
        {
            try
            {
                var lstClient = await objClient.GetAllClients(filter: filter);
                return GetResponseResult(lstClient);
            }
            catch (Exception)
            {
                return UnprocessableEntity(GetResponseResult<ClientList>(null, new UnprocessableEntityObjectResult("Unknown error occured")));
            }
        }

        /// <summary>
        /// Returns client according to provided id
        /// </summary>
        [HttpGet("{id:long:min(1)}", Name = "GetClient")]
        public async Task<ActionResult<ResponseModel<ClientGetModel>>> Get(long id)
        {
            try
            {
                var client = await objClient.GetClient(id);
                return GetResponseResult(client);
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(GetResponseResult<ClientGetModel>(null, new BadRequestObjectResult(ex.Message)));
            }
            catch (Exception)
            {
                return UnprocessableEntity(GetResponseResult<ClientGetModel>(null, new UnprocessableEntityObjectResult("Unknown error occured")));
            }
        }

        /// <summary>
        /// Deletes client all phones according to provided id
        /// </summary>
        [HttpDelete("{id:long:min(1)}", Name = "DelClientPhone")]
        public IActionResult DeletePhone(long id)
        {
            objClient.DeleteClientPhones(id);

            return Ok();
        }

        /// <summary>
        /// Updates particular client
        /// </summary>
        [HttpPut("{id:long:min(1)}")]
        public async Task<ActionResult<ResponseModel<ClientGetModel>>> Put(long id, ClientUpdateModel model)
        {
            try
            {
                objClient.UpdateClient(id, model);

                return GetResponseResult(await objClient.GetClient(id));
            }
            catch (Exception ex)
            {
                return HandleException<ClientGetModel>(exception: ex);
            }
        }

        /// <summary>
        /// Creates new campaign
        /// </summary>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult<ResponseModel<Client>>> Post(ClientCreateModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(GetResponseResult<Client>(null, new BadRequestObjectResult(string.Empty)));
        //        }

        //        var client = await objclient.CreateClient(model);

        //        var url = Url.RouteUrl("GetClient", new { id = client.Id }, Request.Scheme);
        //        return Created(new Uri(url), GetResponseResult(client, new CreatedResult(url, client)));
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleException<Client>(exception: ex);
        //    }
        //}
    }
}