

## DataDog Agent

http://127.0.0.1:5002/


## Instrument your application

$target=[System.EnvironmentVariableTarget]::Process
[System.Environment]::SetEnvironmentVariable("CORECLR_ENABLE_PROFILING","1",$target)
[System.Environment]::SetEnvironmentVariable("CORECLR_PROFILER","{846F5F1C-F9AE-4B07-969E-05C26BC060D8}",$target)
[System.Environment]::SetEnvironmentVariable("COR_ENABLE_PROFILING","1",$target)
[System.Environment]::SetEnvironmentVariable("COR_PROFILER","{846F5F1C-F9AE-4B07-969E-05C26BC060D8}",$target)
 
[System.Environment]::SetEnvironmentVariable("DD_ENV","demo",$target)
[System.Environment]::SetEnvironmentVariable("DD_VERSION","1.0.0",$target)
[System.Environment]::SetEnvironmentVariable("DD_LOGS_INJECTION","true",$target)