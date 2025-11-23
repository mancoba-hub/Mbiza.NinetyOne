using Mbiza.NinetyOne.TopScorers.Application.Commom;
using Mbiza.NinetyOne.TopScorers.Application.DTOs;
using Mbiza.NinetyOne.TopScorers.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mbiza.NinetyOne.TopScorers.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("multipart/form-data")]
    public class TopScorersController : ControllerBase
    {
        #region Fields

        private readonly ITopScorersService _topScorersService;
        private readonly ILogger<TopScorersController> _topScorerslogger;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TopScorersController class with the specified top scorers service and
        /// logger.
        /// </summary>
        /// <param name="topScorersService">The service used to retrieve top scorer data for the controller's actions. Cannot be null.</param>
        /// <param name="topScorerslogger">The logger used to record diagnostic and operational information for this controller. Cannot be null.</param>
        public TopScorersController(ITopScorersService topScorersService, ILogger<TopScorersController> topScorerslogger)
        {
            _topScorersService = topScorersService;
            _topScorerslogger = topScorerslogger;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the top scorers
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("scorers/file/")]
        [ProducesResponseType(typeof(IEnumerable<DtoTopScorer>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DtoTopScorer>>> CreateNewTopScorersByFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0 || Validations.IsValidExtension(file.Name))
                {
                    string message = "No file uploaded or invalid extension.";
                    _topScorerslogger.LogWarning(message);
                    return BadRequest(message);
                }

                var response = _topScorersService.CreateTopScorersAsync(file.OpenReadStream());
                return Ok(response);
            }
            catch (Exception e)
            {
                _topScorerslogger.LogError($"An error occured", e);
                throw;
            }
        }

        /// <summary>
        /// Gets the top scorers
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("scorers/content/")]
        [ProducesResponseType(typeof(IEnumerable<DtoTopScorer>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DtoTopScorer>>> CreateNewTopScorersByContent(string data, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data))
                {
                    string message = "No content uploaded.";
                    return BadRequest(message);
                }

                var response = _topScorersService.CreateTopScorersAsync(data, cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                _topScorerslogger.LogError($"An error occured", e);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the top scorer whose name matches the specified value.
        /// </summary>
        /// <param name="name">The name of the scorer to search for. This value is case-insensitive and cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="DtoTopScorer"/>
        /// object representing the top scorer with the specified name, or <see langword="null"/> if no matching scorer
        /// is found.</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("topScorerByName/{name}")]
        [ProducesResponseType(typeof(DtoTopScorer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DtoTopScorer>> GetTopScorerByName(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    string message = "First or second name was not entered.";
                    return BadRequest(message);
                }

                var response = await _topScorersService.GetTopScorerByNameAsync(name, cancellationToken);
                if (response == null)
                {
                    return NotFound($"Top Scorer with the name {name} cannot be found.");
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                _topScorerslogger.LogError($"An error occured", e);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a collection of top scorers.
        /// </summary>
        /// <remarks>This endpoint returns the current list of top scorers as determined by the underlying
        /// service. The response is provided with HTTP status code 200 (OK) on success.</remarks>
        /// <returns>An <see cref="ActionResult{T}"/> containing an enumerable collection of <see cref="DtoTopScorer"/> objects
        /// representing the top scorers. Returns an empty collection if no top scorers are found.</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("topScorers/")]
        [ProducesResponseType(typeof(IEnumerable<DtoTopScorer>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DtoTopScorer>>> GetTopScorers(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _topScorersService.GetTopScorersAsync(cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                _topScorerslogger.LogError($"An error occured", e);
                throw;
            }
        }

        #endregion
    }
}
