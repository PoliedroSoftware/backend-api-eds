using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.FileUploadS3.Command;
using Poliedro.Eds.Application.FileUploadS3.Query;
using Poliedro.Eds.Domain.FileUploadS3;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class FileUploadS3Endpoints
{
    public static IEndpointRouteBuilder MapFileUploadS3Endpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/files")
            .WithTags("FileUploadS3")
            .RequireAuthorization("AdminOrIslander");

        group.MapPost("upload", UploadFile)
            .WithName("UploadFile")
            .WithSummary("Upload files to S3")
            .DisableAntiforgery()
            .Accepts<UploadFileRequest>("multipart/form-data")
            .Produces(StatusCodes.Status200OK);

        group.MapGet("court/{courtId:int}", GetCourtImages)
            .WithName("GetCourtImages")
            .WithSummary("Get all images for a specific court from S3")
            .Produces<List<string>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        return app;
    }

    private static async Task<IResult> UploadFile(
        [FromForm] UploadFileRequest request,
        IMediator mediator)
    {
        var results = new List<string>();
        foreach (var file in request.Files)
        {
            var command = new UploadFileCommand(file, request.CourtId);
            var result = await mediator.Send(command);
            results.Add(result);
        }
        return TypedResults.Ok(new { Urls = results });
    }

    private static async Task<IResult> GetCourtImages(
        [FromRoute] int courtId,
        IMediator mediator)
    {
        var query = new GetCourtImagesQuery(courtId);
        var result = await mediator.Send(query);

        if (!result.IsSuccess)
        {
            return TypedResults.BadRequest(result.Error);
        }

        return TypedResults.Ok(new { Images = result.Value });
    }
}
