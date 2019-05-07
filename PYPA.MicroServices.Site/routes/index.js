'use strict';
var express = require('express');
var router = express.Router();

const users = {};

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
    if (req.query.ip)
        users[req.query.ip] = items;
    res.render('cart', { title: 'Cart', items: items });
});
router.get('/order', function(req, res) {
    var items = [];
    if (req.query.ip && users[req.query.ip])
        items = users[req.query.ip];
    res.render('order', { title: 'Order', items: items });
});

module.exports = router;