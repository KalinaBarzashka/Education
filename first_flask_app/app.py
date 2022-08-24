from flask import Flask, render_template, request

app = Flask(__name__)

@app.route("/") # when to call this index function - decorator
def index():
    #name = request.args.get("name") #name=name in render_template() # name=name; first name is the name of the variable we want to give to the template and the second is the value
    return render_template("index.html")


@app.route("/greet", method=["POST"])
def greet():
    name = request.args.get("name", "default value") #default value will show only if not provided in url after ?
    return render_template("greet.html", name=name)