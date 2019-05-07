'use strict';
var express = require('express');
var router = express.Router();

/* GET home page. */
router.get('/', function(req, res) {
    res.render('index', { title: 'Loja' });
});

router.get('/home', function(req, res) {
    res.render('home', { title: 'Loja.Home' });
});

router.get('/cart', function(req, res) {
    var items = [];
    if (req.query.item)
        items = req.query.item;
    res.render('cart', { title: 'Cart', items: items });
});

module.exports = router;