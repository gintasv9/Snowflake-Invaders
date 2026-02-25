# Snowflake Invaders

This repository includes a browser port in `web/`.

## Play locally

```bash
python3 -m http.server 4173
# open http://127.0.0.1:4173/web/index.html
```

## Deployment (GitHub Pages)

A GitHub Actions workflow is included at `.github/workflows/deploy-gh-pages.yml` and deploys the `web/` directory.

### One-time setup
1. Push this branch to your GitHub repository.
2. Open **Settings → Pages**.
3. Set **Source** to **GitHub Actions**.
4. Trigger workflow **Deploy Web Edition to GitHub Pages** (or push to `main`, `master`, or `work`).

### Live URL
After the workflow succeeds, use the URL printed in:
- the workflow run **Deploy to GitHub Pages** step output, or
- the workflow run summary section **✅ Snowflake Invaders deployed**.

Typical URL format:

`https://<your-github-username>.github.io/Snowflake-Invaders/`

(Replace `Snowflake-Invaders` if your repository slug differs.)

### If deployment fails with "Resource not accessible by integration"
This means the workflow token does not have permission to enable/read Pages for this repository yet.

Fix:
1. Open **Settings → Pages** and set **Source** to **GitHub Actions**.
2. If this is an organization repository, ensure org policy allows GitHub Actions to deploy Pages.
3. Re-run the workflow.
