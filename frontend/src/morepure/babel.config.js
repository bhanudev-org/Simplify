module.exports = {
  presets: [
    [
      '@babel/preset-env',
      {
        targets: {
          chrome: '58',
          ie: '11',
        },
        modules: false,
      },
    ],
    '@babel/preset-typescript',
  ],
}
