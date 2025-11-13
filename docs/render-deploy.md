# Render deployment guide

1. **Push the repository to GitHub**
   - Include the entire project structure:
     - `backSendify/` (source code, `backSendify.csproj`, `Program.cs`, etc.).
     - `Dockerfile`, `.dockerignore`, and `render.yaml` in the repo root.
     - `scripts/render-entrypoint.sh`.
     - Configuration samples (`appsettings*.json`) without real secrets.
   - Remove local build artifacts (`bin/`, `obj/`) because they are already ignored.

2. **Prepare secrets and configuration**
   - In Render use environment variables that mirror the .NET configuration keys:
     - `ConnectionStrings__DefaultConnection`
     - `Jwt__Key`
     - `Jwt__Issuer`
     - `Jwt__Audience`
   - Never commit real connection strings or JWT keys; store them as Render environment variables or secrets.

3. **Create the Render Web Service**
   - From the Render dashboard select **New > Web Service**.
   - Choose **Git Provider** and connect to the GitHub repository.
   - Select the branch you want Render to track (defaults to `main`).
   - Render detects `render.yaml` automatically; if not, choose **Docker** as the environment and point to the `Dockerfile`.
   - For services created from the blueprint (`render.yaml`), PR previews and auto-deploys are configured automatically.

4. **First deployment**
   - Render builds the Docker image using the provided `Dockerfile`.
   - The container listens on port `8080`, which Render maps to the public URL.
   - After the deploy finishes, open the service URL and verify `/swagger` loads.

5. **Subsequent updates**
   - Push commits to the tracked branch; Render rebuilds and redeploys automatically.
   - Use pull requests if you enable PR previews (requires `render.yaml` in the default branch).
