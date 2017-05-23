const getCaptcha = require('./OcrKing');
const path = require('path');


const filePath = path.resolve('./56623.jpg');

getCaptcha(filePath, {
  apiKey: '51ef8dc7b3b0b5be53qUWQPIpynwaRCZb2MxvzUXqlsPQYPtbp0CG0Df55dh9vp5BScpESjJcH',
}).then((result) => {
  console.log(result);
}).catch((err) => {
  console.log(err);
});
