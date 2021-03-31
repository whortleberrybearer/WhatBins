'use strict';

const { conversation } = require('@assistant/conversation');
const functions = require('firebase-functions');
const request = require('sync-request');
const moment = require('moment');

const app = conversation();

function outputCollections(collections, covn) {
    const now = moment().utc();
    const tomorrow = moment().utc().startOf('day').add(1, 'day');
    const next = moment().utc().startOf('day').add(7, 'days');
    const lookahead = moment().utc().startOf('day').add(7, 'days');
    var collectionFound = false;

    collections.forEach(function (arrayItem) {
        var collectionDate = moment(arrayItem.Date);

        if ((collectionDate > now) && (collectionDate < lookahead)) {
            collectionFound = true;

            var message = "";

            if (collectionDate.isSame(now, 'day')) {
                message = "Today";
            }
            else if (collectionDate.isSame(tomorrow, 'day')) {
                message = "Tomorrow";
            }
            else {
                // This can happen if it is later in the day, so say next to avoid confusion thinking it is today.
                if (collectionDate.isSame(next, 'day')) {
                    message = "Next ";
                }
                else {
                    message = "On ";
                }

                message += collectionDate.format('dddd');
            }

            message += " your ";

            arrayItem.Bins.forEach(function (bin, index) {
                if (index > 0) {
                    if (index === arrayItem.Bins.length - 1) {
                        message += " and ";
                    }
                    else {
                        message += ", ";
                    }
                }

                message += bin.Colour.toLowerCase();

            });

            if (arrayItem.Bins.length === 1) {
                message += " bin ";
            }
            else {
                message += " bins "
            }

            message += "will be collected."

            covn.add(message);
        }
        else if ((collectionDate.isSame(now, 'day'))) {
            // Collection was earlier today, so increase the lookahead to include next weeks.
            lookahead.add(1, 'day');
        }
    });

    if (collectionFound === false) {
        conv.add("There are no collections in the next week.");
    }
};

function outputCollectionState(lookupResult, conv) {
    if (lookupResult.State === "Collection") {
        return outputCollections(lookupResult.Collections, conv);
    }
    else if (lookupResult.State === "NoCollection") {
        conv.add("No collections are available at this address.");
    }

    conv.add("This address is currently not supported.");
};

app.handle('lookupCollections', conv => {

    // Query the lookup for the collection data.  If this is not done syncrounsly, the conversions completes before the response is recieved.
    const lookupUrl = new URL(process.env.LOOKUP_URL);
    lookupUrl.searchParams.append("postcode", process.env.POSTCODE);

    const res = request('GET', lookupUrl.toString());

    if (res.statusCode >= 300) {
        // If an error has occurred, just say it is unsupported.
        conv.add("This address is currently not supported.");
    }
    else {
        outputCollectionState(JSON.parse(res.getBody()), conv);
    }
});

exports.ActionsOnGoogleFulfillment = functions.https.onRequest(app);