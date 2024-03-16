using Auto.WebAPI.Features.PrintTasks.Models;
using FluentResults;

namespace Auto.WebAPI.Services.Interfaces;

interface ISessionLineParser
{
    Result<Session> Parse(string line);
}
