# OpenAI Integration API Documentation

## Overview
This document describes the OpenAI integration that has been added to the Poliedro EDS Backend API.

## API Endpoints

### 1. Send Message to OpenAI
**POST** `/api/v1/openai/chat`

Sends a message to OpenAI GPT models and returns the AI-generated response.

#### Request Body
```json
{
  "userMessage": "Your question or message here",
  "model": "gpt-3.5-turbo",
  "systemMessage": "Optional system message to set context",
  "maxTokens": 1000,
  "temperature": 0.7
}
```

#### Request Parameters
- `userMessage` (string, required): The message to send to the AI
- `model` (string, optional): OpenAI model to use. Default: "gpt-3.5-turbo"
  - Supported models: gpt-3.5-turbo, gpt-3.5-turbo-16k, gpt-4, gpt-4-32k, gpt-4-turbo-preview, gpt-4o, gpt-4o-mini
- `systemMessage` (string, optional): System message to set context for the AI
- `maxTokens` (integer, optional): Maximum tokens in response. Default: 1000, Max: 4000
- `temperature` (double, optional): Creativity level (0-2). Default: 0.7

#### Response
```json
{
  "id": 1,
  "responseContent": "AI-generated response text",
  "model": "gpt-3.5-turbo",
  "tokensUsed": 245,
  "finishReason": "stop",
  "processingTimeMs": 1250.5,
  "createdAt": "2024-01-15T10:30:00Z"
}
```

### 2. Get Chat History
**GET** `/api/v1/openai/history?pageNumber=1&pageSize=10`

Retrieves the chat history (previous conversations) for the authenticated user.

#### Query Parameters
- `pageNumber` (integer, optional): Page number for pagination. Default: 1
- `pageSize` (integer, optional): Number of results per page. Default: 10, Max: 100

#### Response
```json
{
  "statusCode": 200,
  "data": [
    {
      "id": 1,
      "responseContent": "AI response",
      "model": "gpt-3.5-turbo",
      "tokensUsed": 245,
      "finishReason": "stop",
      "processingTimeMs": 1250.5,
      "createdAt": "2024-01-15T10:30:00Z"
    }
  ]
}
```

## Authentication & Authorization

Both endpoints require authentication and use the `AdminOrIslander` policy:
- **Header**: `Authorization: Bearer <JWT_token>`
- **Roles**: Admin or User

## Configuration

### Environment Variables
The OpenAI API key must be configured via:
- Configuration: `OpenAI:ApiKey`
- Environment Variable: `OPENAI_API_KEY`

### Database Tables
The integration creates two new tables:
- `openai_requests`: Stores user requests
- `openai_responses`: Stores AI responses

## Error Responses

### 400 Bad Request
```json
{
  "statusCode": 400,
  "error": "ValidationFailed: User message is required"
}
```

### 401 Unauthorized
```json
{
  "statusCode": 401,
  "error": "The request lacks valid authentication credentials"
}
```

### 500 Internal Server Error
```json
{
  "statusCode": 500,
  "error": "OpenAIError: API rate limit exceeded"
}
```

## Technical Implementation

### Architecture
The implementation follows the Clean Architecture pattern:

1. **Domain Layer**: 
   - `OpenAIRequestEntity` and `OpenAIResponseEntity`
   - Domain services and repository interfaces

2. **Application Layer**:
   - CQRS with MediatR
   - Commands: `SendOpenAIMessageCommand`
   - Queries: `GetChatHistoryQuery`
   - Validation with FluentValidation

3. **Infrastructure Layer**:
   - OpenAI client integration
   - Entity Framework repositories
   - Database configurations

4. **API Layer**:
   - RESTful controller with Swagger documentation
   - JWT authentication and authorization

### Features
- ✅ Request/Response logging to database
- ✅ Comprehensive validation
- ✅ Error handling and logging
- ✅ Pagination for chat history
- ✅ Multiple OpenAI model support
- ✅ Configurable parameters (temperature, max tokens)
- ✅ JWT authentication integration
- ✅ Swagger API documentation

## Usage Examples

### Send a simple question
```bash
curl -X POST "https://api.example.com/api/v1/openai/chat" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "userMessage": "Explain what is clean architecture in software development"
  }'
```

### Send with custom parameters
```bash
curl -X POST "https://api.example.com/api/v1/openai/chat" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "userMessage": "Write a creative story about AI",
    "model": "gpt-4",
    "systemMessage": "You are a creative storyteller",
    "maxTokens": 2000,
    "temperature": 1.2
  }'
```

### Get chat history
```bash
curl -X GET "https://api.example.com/api/v1/openai/history?pageNumber=1&pageSize=5" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## Notes
- All requests and responses are logged in the database for audit purposes
- The user ID is extracted from the JWT token for user-specific chat history
- Processing time is measured and stored for performance monitoring
- The integration supports all major OpenAI GPT models