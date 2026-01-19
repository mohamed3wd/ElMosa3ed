# 🚀 Deploy ElMosa3ed Backend to Railway.app

## Prerequisites
- GitHub account
- Railway.app account (sign up at https://railway.app)

## Step 1: Push to GitHub
1. Create a new GitHub repository
2. Initialize git in the backend folder:
   ```bash
   cd backend
   git init
   git add .
   git commit -m "Initial commit"
   git branch -M main
   git remote add origin YOUR_GITHUB_REPO_URL
   git push -u origin main
   ```

## Step 2: Deploy to Railway
1. Go to https://railway.app
2. Click "Start a New Project"
3. Select "Deploy from GitHub repo"
4. Authorize Railway to access your GitHub
5. Select your backend repository
6. Railway will automatically detect the Dockerfile and deploy

## Step 3: Configure Environment Variables
In Railway dashboard, add these variables:
- `ASPNETCORE_ENVIRONMENT` = `Production`
- `Gemini__Key` = `AIzaSyDJpYuumayvNP5-G9fNQ0uozVlKGSgPKh4`
- `Gemini__Model` = `gemini-1.5-flash`

## Step 4: Get Your API URL
1. After deployment, Railway will provide a public URL
2. Copy this URL (e.g., https://your-app.railway.app)
3. Update your Chrome extension's API_URL in popup.js

## Step 5: Test Your API
Visit: https://your-app.railway.app/swagger

## 🎉 Done!
Your backend is now live and accessible from anywhere!