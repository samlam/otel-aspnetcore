# Issue with rc3

Issue sending metric with the package v1.2.0-rc3 and here is the fix.

```csharp
// Startup.cs
    .AddOtlpExporter( (OtlpExporterOptions otlp, MetricReaderOptions reader) =>
    {
        otlp.Endpoint = new Uri("http://localhost:4317");
        otlp.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        reader.MetricReaderType = MetricReaderType.Periodic;
    });
```

```xml
<!-- core-api.csproj -->
<PackageReference Include="OpenTelemetry" Version="1.2.0-rc3" />
<PackageReference Include="OpenTelemetry.Exporter.OpenTelemetryProtocol" Version="1.2.0-rc3" />
<PackageReference Include="OpenTelemetry.Exporter.Prometheus" Version="1.2.0-rc3" />
```

## To Run via CLI

```sh
dotnet run
```

## To Run via VSCode

F5 to launch the site

## Assumption

OTEL Collector runs locally `localhost:4317`
