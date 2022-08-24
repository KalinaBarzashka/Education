from flask import Flask, render_template, request

app = Flask(__name__)

@app.route("/") # when to call this index function - decorator
def index():
    return render_template("index.html")