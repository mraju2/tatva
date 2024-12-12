"use client";
import React, { useState } from "react";
import {
  IconArrowRight,
  IconDatabase,
  IconCode,
  IconRobot,
  IconMessageCircle,
  IconListDetails,
  IconTable,
} from "@tabler/icons-react";

const TatvaHomepage = () => {
  const [email, setEmail] = useState("");

  return (
    <div className="bg-slate-950 text-white min-h-screen flex flex-col">
      {/* Navigation */}
      <nav className="px-6 py-4 flex justify-between items-center">
        <div className="flex items-center space-x-2">
          <IconRobot className="text-emerald-400" size={32} />
          <span className="text-2xl font-bold text-white">Tatva</span>
        </div>
        <div className="space-x-4">
          <a href="#" className="text-slate-300 hover:text-white">
            Docs
          </a>
          <a href="#" className="text-slate-300 hover:text-white">
            GitHub
          </a>
          <button className="bg-emerald-600 text-white px-4 py-2 rounded-lg hover:bg-emerald-700 transition">
            Get Started
          </button>
        </div>
      </nav>

      {/* Hero Section */}
      <main className="flex-grow container mx-auto px-6 py-16 text-center">
        <h1 className="text-5xl font-bold mb-6 leading-tight">
          Ask Questions, Get Answers
          <br />
          Powered by SQL and AI
        </h1>
        <p className="text-xl text-slate-300 mb-8 max-w-2xl mx-auto">
          Tatva bridges the gap between natural language and data querying. Ask
          questions in plain English, and let our AI-powered system retrieve
          insights from your database.
        </p>

        {/* Email Input */}
        <div className="max-w-md mx-auto flex items-center space-x-2 mb-8">
          <input
            type="email"
            placeholder="Enter your email"
            className="flex-grow px-4 py-3 bg-slate-800 text-white rounded-lg border border-slate-700 focus:outline-none focus:ring-2 focus:ring-emerald-500"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
          <button className="bg-emerald-600 text-white px-6 py-3 rounded-lg hover:bg-emerald-700 transition">
            <IconArrowRight size={20} />
          </button>
        </div>

        {/* Features */}
        <div className="grid md:grid-cols-4 gap-6 max-w-4xl mx-auto mt-16">
          {[
            {
              icon: (
                <IconDatabase className="text-emerald-400 mb-4" size={40} />
              ),
              title: "Multiple Data Sources",
              description:
                "Connect to SQL Server, PostgreSQL, MySQL, and more to retrieve data seamlessly.",
            },
            {
              icon: (
                <IconMessageCircle
                  className="text-emerald-400 mb-4"
                  size={40}
                />
              ),
              title: "Natural Language Queries",
              description:
                "Ask questions in plain English, and let AI generate precise SQL queries.",
            },
            {
              icon: <IconCode className="text-emerald-400 mb-4" size={40} />,
              title: "Seamless Execution",
              description:
                "Run queries on your database and retrieve results in real time.",
            },
            {
              icon: <IconRobot className="text-emerald-400 mb-4" size={40} />,
              title: "User-Friendly Results",
              description:
                "Get answers presented in simple and understandable formats.",
            },
          ].map((feature, index) => (
            <div
              key={index}
              className="bg-slate-900 p-6 rounded-lg text-center border border-slate-800"
            >
              {feature.icon}
              <h3 className="text-xl font-semibold mb-3">{feature.title}</h3>
              <p className="text-slate-400">{feature.description}</p>
            </div>
          ))}
        </div>

        {/* How It Works Section */}
        <div className="max-w-5xl mx-auto mt-24 text-center">
          <h2 className="text-4xl font-bold mb-6">How It Works</h2>
          <div className="grid md:grid-cols-3 gap-8">
            {[
              {
                icon: (
                  <IconListDetails
                    className="text-emerald-400 mb-4"
                    size={40}
                  />
                ),
                step: "Step 1: Ask",
                description:
                  "Type your question in plain English, specifying the data source or leaving it open.",
              },
              {
                icon: <IconTable className="text-emerald-400 mb-4" size={40} />,
                step: "Step 2: Translate",
                description:
                  "Our AI converts your question into an optimized SQL query.",
              },
              {
                icon: <IconRobot className="text-emerald-400 mb-4" size={40} />,
                step: "Step 3: Retrieve",
                description:
                  "Tatva executes the query, fetches results, and presents them in a user-friendly format.",
              },
            ].map((step, index) => (
              <div
                key={index}
                className="bg-slate-900 p-6 rounded-lg text-center border border-slate-800"
              >
                {step.icon}
                <h3 className="text-xl font-semibold mb-3">{step.step}</h3>
                <p className="text-slate-400">{step.description}</p>
              </div>
            ))}
          </div>
        </div>
      </main>

      {/* Footer */}
      <footer className="bg-slate-900 py-8 border-t border-slate-800">
        <div className="container mx-auto px-6 text-center">
          <div className="flex justify-center space-x-6 mb-6">
            <a href="#" className="text-slate-400 hover:text-white">
              GitHub
            </a>
            <a href="#" className="text-slate-400 hover:text-white">
              Twitter
            </a>
            <a href="#" className="text-slate-400 hover:text-white">
              Discord
            </a>
          </div>
          <p className="text-slate-500">© 2024 Tatva. All rights reserved.</p>
        </div>
      </footer>
    </div>
  );
};

export default TatvaHomepage;
