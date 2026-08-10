// 1. HTTP request hits Controller action
//    e.g. JobsController.GetJobOrder(JobOrderId, RespondentId)
//         │
//         │  MVC model binding fills the parameters (route/query/body)
//         │  ValidateModelStateAttribute + FluentValidation already ran
//         │  by this point (before the action body even executes)
//         ▼
// 2. Controller builds a Query/Command object — plain data, no logic
//    e.g. new GetJobOrderQuery(JobOrderId, RespondentId)
//         ▼
// 3. Controller calls _mediator.Send(query)
//         │
//         │  MediatR looks at the query's runtime type, finds the ONE
//         │  IRequestHandler<TQuery, TResponse> registered for it at
//         │  startup (via AddMediatR's assembly scan), and resolves it
//         │  from the DI container.
//         ▼
// 4. DI constructs the matching Handler
//    e.g. GetJobOrderQueryHandler
//         │
//         │  The Handler's own constructor asks DI for whatever Service
//         │  interface it needs — DI resolves that too.
//         ▼
// 5. DI injects the Service implementation into the Handler
//    e.g. IJobOrdersService  →  JobOrdersService  (AddScoped, registered
//         in DIServices.cs at startup)
//         ▼
// 6. Handler.Handle() calls straight into the Service method
//    e.g. _jobOrdersService.GetJobOrder(JobOrderId, RespondentId)
//         │
//         │  THIS is where the real work happens — ES lookups, EF Core
//         │  DbContext queries, other business-layer collaborators —
//         │  and where a DTO gets built (e.g. JobOrderItem).
//         ▼
// 7. That DTO bubbles straight back up, unchanged, through:
//    Service → Handler.Handle() → Send() → the controller's local variable
//         ▼
// 8. Controller wraps it: return Ok(dto)
//         ▼
// 9. ASP.NET Core serializes it to JSON and sends the HTTP response