module.exports = ctx => ({
    map: ctx.env === 'development' ? ctx.map : false,
    plugins: {
        'postcss-import': {},
        'postcss-nested': {},
        'postcss-preset-env': { stage: 1 },
        cssnano: ctx.env === 'development' ? false : { preset: ['default', { discardComments: { removeAll: true } }] },
    },
})
