const xml2json = require('node-xml2json');
const request = require('request');
const fs = require('fs');
const url = require('url');

function getCaptcha(filePath, option) {
  const defaultOption = {
    service: 'OcrKingForPhoneNumber',
    language: 'eng',
    charset: '11',
    type: 'http://www.unknown.com',
  };
  const result = url.format({
    protocol: 'http:',
  	hostname: 'lab.ocrking.com/ok.html',
    query: Object.assign({}, defaultOption, option),
  });

  return new Promise((resolve, reject) => {
    const formData = {
      file: fs.createReadStream(filePath),
    };

    request.post({
      url: result,
      formData,
    }, (err, httpResponse, body) => {
      if (err) {
        reject({
          error: err,
        });
      }
      try {
        bodyObj = xml2json.parser(body).results.resultlist || {};
        resolve(bodyObj);
      } catch (e) {
        reject({
          error: e,
        });
      }
    });
  });
}

module.exports = getCaptcha;
