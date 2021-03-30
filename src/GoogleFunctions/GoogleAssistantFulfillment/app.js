'use strict';
try {
const { conversation } = require('@assistant/conversation');
const functions = require('firebase-functions');
const http = require('https');
const querystring = require('querystring');
const request1 = require('sync-request');

const app = conversation();

    function somethingelse(collections) {

    };


    function something(lookupResult) {
        if (lookupResult.State === "Collection") {
            return somethingelse(lookupResult.Collections);
        }
        else if (lookupResult.State === "NoCollection") {
            return "No collections are available at this address";
        }

        return "This address is currently not supported.";
    };


    const parameters = {
        postcode: "PR7 7HB"
    };

    const options = {
        hostname: "https://europe-west2-what-bins-708d7.cloudfunctions.net",
        port: 443,
        path: "/lookup?" + querystring.stringify(parameters),
        method: 'GET'
    };



    //const res1 = request1('GET', "https://europe-west2-what-bins-708d7.cloudfunctions.net/lookup?postcode=PR7%207HB");

    //var user = JSON.parse(res1.getBody('utf8'));

    var data = "{\"State\":\"Collection\",\"Collections\":[{\"Date\":\"2021-04-02\",\"Bins\":[{\"Colour\":\"Green\"}]},{\"Date\":\"2021-04-09\",\"Bins\":[{\"Colour\":\"Blue\"},{\"Colour\":\"Grey\"},{\"Colour\":\"Brown\"}]},{\"Date\":\"2021-04-16\",\"Bins\":[{\"Colour\":\"Green\"}]},{\"Date\":\"2021-04-23\",\"Bins\":[{\"Colour\":\"Blue\"},{\"Colour\":\"Grey\"}]},{\"Date\":\"2021-04-30\",\"Bins\":[{\"Colour\":\"Green\"}]},{\"Date\":\"2021-05-07\",\"Bins\":[{\"Colour\":\"Blue\"},{\"Colour\":\"Grey\"},{\"Colour\":\"Brown\"}]},{\"Date\":\"2021-05-14\",\"Bins\":[{\"Colour\":\"Green\"}]},{\"Date\":\"2021-05-21\",\"Bins\":[{\"Colour\":\"Blue\"}]}]}";


    const lookupResult = JSON.parse(data);

    ssomething(lookupResult);

    var i = 0;

    //const request = http.request("https://europe-west2-what-bins-708d7.cloudfunctions.net/lookup?postcode=PR7%207HB", (response) => {
    //    console.log(`statusCode: ${res.statusCode}`);

    //    res.on('data', d => {
    //        process.stdout.write(d)
    //    });

    //    // Implement your code here
    //    conv.add("woo.s  i looks like that woked");
    //});

    //request.on('error', (error) => {
    //    console.log(error.message);

    //    conv.add("doh, thats not good");
    //});

    //request.end(() => {
    //    conv.add("it ended");
    //});


    //while (!request.finished) {
    //    console.log("waiting");
    //}

    //// Implement your code here
    //conv.add("we got to the wne");

}
catch (e) {
    console.error(e);
}


