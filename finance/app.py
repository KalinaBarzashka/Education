import os

from cs50 import SQL
from flask import Flask, flash, redirect, render_template, request, session
from flask_session import Session
from tempfile import mkdtemp
from werkzeug.security import check_password_hash, generate_password_hash
from datetime import datetime

from helpers import apology, login_required, lookup, usd

# Configure application
app = Flask(__name__)

# Ensure templates are auto-reloaded
app.config["TEMPLATES_AUTO_RELOAD"] = True

# Custom filter
app.jinja_env.filters["usd"] = usd

# Configure session to use filesystem (instead of signed cookies)
app.config["SESSION_PERMANENT"] = False
app.config["SESSION_TYPE"] = "filesystem"
Session(app)

# Configure CS50 Library to use SQLite database
db = SQL("sqlite:///finance.db")

# Make sure API key is set
# if not os.environ.get("API_KEY"):
#     raise RuntimeError("API_KEY not set")


@app.after_request
def after_request(response):
    """Ensure responses aren't cached"""
    response.headers["Cache-Control"] = "no-cache, no-store, must-revalidate"
    response.headers["Expires"] = 0
    response.headers["Pragma"] = "no-cache"
    return response


@app.route("/")
@login_required
def index():
    """Show portfolio of stocks"""

    # query db for users shares, group by user id and symbol
    data = db.execute(
        "SELECT symbol, SUM(shares) as shares, AVG(share_price) as share_price FROM transactions WHERE user_id = ? GROUP BY symbol HAVING shares > 0", session["user_id"])
    # init total variable to get total sum of users money (in cash and in stock)
    total_sum = 0
    # add data about symbol full name and total price of the shares that user bought
    for d in data:
        stock_data = lookup(d["symbol"])
        d["name"] = stock_data["name"]
        d["price"] = usd(stock_data["price"])
        d["total_price"] = usd(d["shares"] * stock_data["price"])
        total_sum = total_sum + (d["shares"] * stock_data["price"])

    # get current user cash
    current_user_price = db.execute("SELECT cash FROM users WHERE id = ?", session["user_id"])[0]["cash"]

    total_sum = total_sum + current_user_price

    return render_template("index.html", data=data, current_user_price=usd(current_user_price), total_sum=usd(total_sum))


@app.route("/buy", methods=["GET", "POST"])
@login_required
def buy():
    """Buy shares of stock"""

    # User reached route via POST (as by submitting a form via POST)
    if request.method == "POST":

        # missings = is_provided("symbol") or is_provided("shares")
        # if not missings:
        # return apology("must provide symbol/shares", 400)
        symbol = request.form.get("symbol")
        if not symbol:
            return apology("must provide symbol", 400)

        tmp_shares = request.form.get("shares")
        if not tmp_shares:
            return apology("must provide share count", 400)

        if not tmp_shares.isdigit():
            return apology("invalid number of shares", 400)

        shares = float(tmp_shares)
        if not shares.is_integer():
            return apology("must provide whole number for shares", 400)

        if shares <= 0:
            return apology("shares must be positive number", 400)

        stock_data = lookup(symbol)
        if not stock_data:
            return apology("not valid symbol", 400)

        users_price = db.execute("SELECT cash FROM users WHERE id = ?", session["user_id"])[0]["cash"]
        total_price = stock_data["price"]*int(shares)

        if total_price >= users_price:
            return apology("not enoght money", 400)

        db.execute("INSERT INTO transactions (user_id, symbol, shares, share_price, event_date) VALUES (?, ?, ?, ?, ?)",
                   session["user_id"], stock_data["symbol"], shares, stock_data["price"], datetime.now())
        db.execute("UPDATE users SET cash = ? WHERE id = ?", users_price - total_price, session["user_id"])
        flash("Bought!")
        return redirect("/")

    # User reached route via GET (as by clicking a link or via redirect)
    else:
        return render_template("buy.html")


@app.route("/history")
@login_required
def history():
    """Show history of transactions"""
    transactions = db.execute(
        "SELECT symbol, shares, share_price as price, event_date FROM transactions WHERE user_id = ?", session["user_id"])

    for t in range(len(transactions)):
        transactions[t]["price"] = usd(transactions[t]["price"])

    return render_template("history.html", transactions=transactions)


@app.route("/login", methods=["GET", "POST"])
def login():
    """Log user in"""

    # Forget any user_id
    session.clear()

    # User reached route via POST (as by submitting a form via POST)
    if request.method == "POST":

        # Ensure username was submitted
        if not request.form.get("username"):
            return apology("must provide username", 403)

        # Ensure password was submitted
        elif not request.form.get("password"):
            return apology("must provide password", 403)

        # Query database for username
        rows = db.execute("SELECT * FROM users WHERE username = ?", request.form.get("username"))

        # Ensure username exists and password is correct
        if len(rows) != 1 or not check_password_hash(rows[0]["hash"], request.form.get("password")):
            return apology("invalid username and/or password", 403)

        # Remember which user has logged in
        session["user_id"] = rows[0]["id"]

        # Redirect user to home page
        return redirect("/")

    # User reached route via GET (as by clicking a link or via redirect)
    else:
        return render_template("login.html")


@app.route("/logout")
def logout():
    """Log user out"""

    # Forget any user_id
    session.clear()

    # Redirect user to login form
    return redirect("/")


@app.route("/quote", methods=["GET", "POST"])
@login_required
def quote():
    """Get stock quote."""

    if request.method == "POST":
        symbol = request.form.get("symbol")
        if not symbol:
            return apology("must provide symbol", 400)

        data = lookup(symbol)

        if data == None:
            return apology("no data found for provided symbol", 400)

        data["price"] = usd(data["price"])

        return render_template("quoted.html", data=data)

    # User reached route via GET (as by clicking a link or via redirect)
    else:
        return render_template("quote.html")


@app.route("/register", methods=["GET", "POST"])
def register():
    """Register user"""

    if request.method == "POST":

        # Return apology if username is blank
        username = request.form.get("username")
        if not username:
            return apology("must provide username", 400)

        # Return apology if password is blank
        password = request.form.get("password")
        confirmation = request.form.get("confirmation")
        if not password or not confirmation:
            return apology("missing password", 400)

        if password != confirmation:
            return apology("passwords don't match", 400)

        # Query to see if username is taken
        rows = db.execute("SELECT * FROM users WHERE username = ?", username)

        if len(rows) != 0:
            return apology("username already taken", 400)

        hash = generate_password_hash(password, "sha256")
        db.execute("INSERT INTO users (username, hash) VALUES (?, ?)", username, hash)

        # Redirect user to login page
        flash("Registered!")
        return redirect("/login")

    # User reached route via GET (as by clicking a link or via redirect)
    else:
        return render_template("register.html")


@app.route("/sell", methods=["GET", "POST"])
@login_required
def sell():
    """Sell shares of stock"""

    user_id = session["user_id"]
    # User reached route via POST (as by submitting a form via POST)
    if request.method == "POST":
        symbol = request.form.get("symbol")
        shares = request.form.get("shares")

        if not symbol:
            return apology("must provide symbol", 400)

        if not shares:
            return apology("must provide shares", 400)

        stock_data = lookup(symbol)
        current_price = stock_data["price"]
        current_symbol = stock_data["symbol"]
        if not current_price:
            return apology("invalid symbol")

        current_user_shares = db.execute(
            "SELECT SUM(shares) as sum FROM transactions WHERE user_id = ? and symbol = ? GROUP BY user_id, symbol", user_id, symbol)

        if not current_user_shares:
            return apology("you does not own that much stocks", 400)

        if current_user_shares[0]["sum"] < int(shares):
            return apology("you does not own that much stocks", 400)

        total_sold_price = current_price * int(shares)

        # Add money to users account
        db.execute("UPDATE users SET cash = cash + ? WHERE id = ?", total_sold_price, user_id)

        # remove stocks from account
        db.execute("INSERT INTO transactions (user_id, symbol, shares, share_price, event_date) VALUES (?, ?, ?, ?, ?)",
                   session["user_id"], current_symbol, (-1)*int(shares), current_price, datetime.now())

        flash("Sold!")
        return redirect("/")

    # User reached route via GET (as by clicking a link or via redirect)
    else:
        stocks = db.execute(
            "SELECT symbol, SUM(shares) as shares FROM transactions WHERE user_id = ? GROUP BY user_id, symbol", user_id)

        return render_template("sell.html", stocks=stocks)
