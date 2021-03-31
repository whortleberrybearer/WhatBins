# WhatBins
A simple Google Assistant action that will lookup the bin collection information from Chorley Council.

## Development Environment
The only dependency needed for development is [Node.js](https://nodejs.org/en/download/).

## Deployment
Deployment to Google is a manual process.  Google will only build the project specified, so the Lookup project will fail to build due to the references to other projects.

For the Lookup project, add all the required source files to the project (don't forget to add the nuget dependencies to to the project file), zip it and upload to storage.

As the Fulfillment project only contains 1 file, just overwrite the file in the function.

## Configuration
### Environment Variables
| Variable   | Description                     | Project     |
|------------|---------------------------------|-------------|
| POSTCODE   | The postcode to lookup.         | Fulfillment |
| LOOKUP_URL | The URL of the lookup function. | Fulfillment |
