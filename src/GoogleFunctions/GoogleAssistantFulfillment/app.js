'use strict';
try {
const { conversation } = require('@assistant/conversation');
const functions = require('firebase-functions');
const http = require('https');
const querystring = require('querystring');
const request1 = require('sync-request');
    const moment = require('moment');

const app = conversation();

    function somethingelse(collections) {
        var now = moment().utc("2021-03-30T20:00:00");
        var tommorow = moment().utc().startOf('day').add(1, 'day');
        var next = moment().utc().startOf('day').add(7, 'days');
        var lookahead = moment().utc().startOf('day').add(7, 'days');

        var collectionFound = false;

        collections.forEach(function (arrayItem) {

            var collectionDate = moment.utc(arrayItem.Date).add(15, 'hours');

            if ((collectionDate > now) && (collectionDate < lookahead)) {
                collectionFound = true;

                var dayOfWeek = collectionDate.format('dddd');

                var thing = "";

                if (collectionDate.isSame(now, 'day')) {
                    // check if today
                    thing = "Today";
                }
                else if (collectionDate.isSame(tommorow, 'day')) {
                    // check if tomorrow
                    thing = "Tomorrow";
                }
                else {
                    // This can happen if it is after the cutoff, so say next to avoid confusion thinking it is today.
                    if (collectionDate.isSame(next, 'day')) {
                        thing = "Next ";
                    }
                    else {
                        thing = "On ";
                    }

                    // The day of the week.
                    thing += collectionDate.format('dddd');
                }

                thing += " your ";

                arrayItem.Bins.forEach(function (bin, index) {

                    if (index > 0) {
                        if (index === arrayItem.Bins.length - 1) {
                            thing += " and ";
                        }
                        else {
                            thing += ", ";
                        }
                    }

                    thing += bin.Colour;

                });

                if (arrayItem.Bins.length === 1) {
                    thing += " bin ";
                }
                else {
                    thing += " bins "
                }

                thing += "will be collected."
            }
            else if ((collectionDate.isSame(now, 'day'))) {
                // Collection was earlier today, so increase the lookahead and use next.
                lookahead.add(1, 'day');
            }
        });

        if (collectionFound === false) {
            var thing = "There are no collections in the next week";
        }
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

    var f = moment().startOf('day');

    //const res1 = request1('GET', "https://europe-west2-what-bins-708d7.cloudfunctions.net/lookup?postcode=PR7%207HB");

    //var user = JSON.parse(res1.getBody('utf8'));

    var data = "{\"State\":\"Collection\",\"Collections\":[{\"Date\":\"2021-03-30\",\"Bins\":[{\"Colour\":\"Green\"}]},{\"Date\":\"2021-04-06\",\"Bins\":[{\"Colour\":\"Blue\"},{\"Colour\":\"Grey\"},{\"Colour\":\"Brown\"}]},{\"Date\":\"2021-04-16\",\"Bins\":[{\"Colour\":\"Green\"}]},{\"Date\":\"2021-04-23\",\"Bins\":[{\"Colour\":\"Blue\"},{\"Colour\":\"Grey\"}]},{\"Date\":\"2021-04-30\",\"Bins\":[{\"Colour\":\"Green\"}]},{\"Date\":\"2021-05-07\",\"Bins\":[{\"Colour\":\"Blue\"},{\"Colour\":\"Grey\"},{\"Colour\":\"Brown\"}]},{\"Date\":\"2021-05-14\",\"Bins\":[{\"Colour\":\"Green\"}]},{\"Date\":\"2021-05-21\",\"Bins\":[{\"Colour\":\"Blue\"}]}]}";


    const lookupResult = JSON.parse(data);

    something(lookupResult);

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


