from flask import Flask, render_template, request

app = Flask(__name__)


@app.route("/")
def index():
    return render_template("index.html")


@app.route("/register", methods=["POST"])
def register():
    # Validate submission
    if not request.form.get("name") or request.form.get("sport") not in ["Basketball", "Soccer", "Ultimate Frisbee"]:
        return render_template("failure.html")

    # Confirm registration
    return render_template("success.html")