const path = require('path')

module.exports = {
  mode: 'production',
  entry: {
    bundle: './src/app.ts',
  },
  resolve: {
    extensions: ['.ts', '.tsx', '.js', '.json'],
  },
  module: {
    rules: [
      {
        test: /\.(ts|js)x?$/,
        exclude: /node_modules/,
        loader: 'babel-loader',
      },
    ],
  },
  plugins: [],
  output: {
    path: path.resolve(__dirname, '../wwwroot', 'assets'),
    filename: '[name].js',
  },
}
