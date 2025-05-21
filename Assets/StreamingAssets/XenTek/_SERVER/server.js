const express = require('express');
const app = express();
app.get('/api/updates', (req, res) => {
    res.json({ project: req.query.project, version: "0.1.1" });
});
app.listen(8080, () => console.log('Server running on http://localhost:8080'));