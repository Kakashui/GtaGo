var path = require('path');

module.exports = { 
  entry: './src/index.js',
  optimization: {
    // We no not want to minimize our code.
    minimize: true
  },
  output: {
    filename: 'clientside.js',
    path: path.resolve(__dirname, '../client_packages/')
  }
};
