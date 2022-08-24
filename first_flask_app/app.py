import flask import Flask, render_template, request

app = Flask(__name__)

@app.route("/") # when to call this index function
def index():
    return render_template("index.html")