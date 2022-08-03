using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Strona_v2.Server.Data.SqlData.File;
using Strona_v2.Server.Data.TagData;
using Strona_v2.Shared.File;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Strona_v2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagModelController : ControllerBase
    {
        private readonly ILogger<TagModelController> _logger;
        private readonly IHashids _Ihashids;
        private readonly ITagSql _ITagSql;
        private readonly IFileToSQL _IFileToSQL;

        public TagModelController(ILogger<TagModelController> logger, IHashids Ihashids, ITagSql iTagSql, IFileToSQL iFileToSQL)
        {
            _logger = logger;
            _Ihashids = Ihashids;
            _ITagSql = iTagSql;
            _IFileToSQL = iFileToSQL;
        }


        // GET: api/<TagModelController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //pobieranie obiektów z tym samych tagiem
        // GET api/<TagModelController>/5
        [HttpGet("{tag}")]
        public async Task<IActionResult> GetTagConcreteList(string tag)
        {
            var tagList = await _ITagSql.GetFileIdFromTags(tag);
            List<FileModelPublic> fileModels = new();
            foreach (var item in tagList)
            {
                fileModels.Add(FileModelPublic.SimpeCast(await _IFileToSQL.GetFileModelsSimpleOne(item),_Ihashids));
            }
            return Ok(fileModels);
        }

        // POST api/<TagModelController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IList<TagModelClient> clients)
        {
            return Ok();
        }

        // PUT api/<TagModelController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<TagModelController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
