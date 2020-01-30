using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PokeApiNet;
using PokeFinder.Entities;

namespace PokeFinderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
      [HttpGet]
        public async Task<ActionResult<dynamic>> Get()
        {
            PokeApiClient pokeClient = new PokeApiClient();
            try
            {
                var pokemons = await pokeClient.GetNamedResourcePageAsync<Pokemon>(964, 0);
                return Ok(pokemons.Results.Select(r => r.Name));

            }
            catch (Exception e)
            {
                return StatusCode(500,e.Message);
            }
            finally
            {
                pokeClient.Dispose();
            }
            

        }


        [HttpGet]
        [Route("download/{pokemon}")]

        public async Task<ActionResult<dynamic>> Download(string pokemon)
        {
            PokeApiClient pokeClient = new PokeApiClient();
            try
            {
                var currentPokemon = await pokeClient.GetResourceAsync<Pokemon>(pokemon);
                var json = JsonConvert.SerializeObject(currentPokemon);


                return File(Encoding.UTF8.GetBytes(json), "text/plain", $"{currentPokemon.Name}-detail.txt");


            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            finally
            {
                pokeClient.Dispose();
            }


        }
    }
}