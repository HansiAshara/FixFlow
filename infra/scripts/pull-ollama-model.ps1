# Helper script to pull the default Ollama model for local development
Write-Host "Pulling default AI model 'llama3.2:3b' into Ollama container..." -ForegroundColor Cyan
docker exec -it fixflow_ollama ollama pull llama3.2:3b
Write-Host "Model ready! FixFlow AI background analysis is configured." -ForegroundColor Green
