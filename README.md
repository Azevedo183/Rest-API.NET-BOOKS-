# REST API example application

This is an example of a backend programmed in .NET


## Docker Image Build

    docker build -t rest-api .
    
    

## Run Docker File

    docker-compose up -d

## Docker Compose
 
You can run this white docker compose after you build the rest-api image
    
    version: '3.8'

        services:
            rest-api:
            image: rest-api
            container_name: my-rest-api
            ports:
                - "8080:80"
            environment:
              - ASPNETCORE_ENVIRONMENT=Development

# REST API

The REST API to the example app is described below.

## Get list of Books

### Request

`GET /api/books`

    curl -X 'GET' \'http://localhost:5079/api/books' \-H 'accept: text/plain'

### Response

    content-type: application/json; charset=utf-8 
    server: Kestrel 
    transfer-encoding: chunked 
    
    [
        {
            "id": 1,
            "title": "To Kill a Mockingbird",
            "author": "Harper Lee",
            "description": "A novel about the serious issues of rape and racial inequality.",
            "published_year": 1960
        }
    ]

## Create a new Book

### Request

`POST /api/books

    curl -X 'POST' \'http://localhost:5079/api/books' \-H 'accept: */*' \-H 'Content-Type: application/json' \-d '{"id": 0,"title": "string","author": "string","description": "string","published_year": 0}'

### Response

    content-type: application/json; charset=utf-8 
    location: http://localhost:5079/api/books?id=6 
    server: Kestrel 
    transfer-encoding: chunked 

    {
    "id": 6,
    "title": "string",
    "author": "string",
    "description": "string",
    "published_year": 0
    }

## Get a specific Thing
On this request they will provide some extra data from the Open Library Search API.
### Request

`GET /api/books/{id}

    curl -X 'GET' \'http://localhost:5079/api/books/1' \-H 'accept: */*'

### Response

    content-type: application/json; charset=utf-8 
    server: Kestrel 
    transfer-encoding: chunked 

    {"localBook": {
    "id": 1,
    "title": "To Kill a Mockingbird",
    "author": "Harper Lee",
    "description": "A novel about the serious issues of rape and racial inequality.",
    "published_year": 1960
    },"openLibraryData": 

## Get info from Open Library Search API

### Request

`GET /api/books/search

    curl -X 'GET' \'http://localhost:5079/api/books/search?query=Memorial%20do%20Convento' \ -H 'accept: */*'

### Response

    content-length: 8764 
    content-type: application/json 
    server: Kestrel 

    {
        "TitleResults": {
            "numFound": 17,
            "start": 0,
            "numFoundExact": true,
            "num_found": 17,
            "documentation_url": "https://openlibrary.org/dev/docs/api/search",
            "q": "",
            "offset": null,
            "docs": [...]
    }
