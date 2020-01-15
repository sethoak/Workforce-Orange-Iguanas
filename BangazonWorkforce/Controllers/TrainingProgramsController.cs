﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonWorkforce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonWorkforce.Controllers
{
    public class TrainingProgramsController : Controller
    {
        //Config and connection

        private readonly IConfiguration _config;

        public TrainingProgramsController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: TrainingPrograms
        public async Task<ActionResult> Index()
        {

            string sql = @"
                        SELECT t.Id,
                        t.[Name],
                        t.Startdate,
                        t.EndDate,
                        t.MaxAttendees
                        FROM TrainingProgram t
                        WHERE StartDate > @Today";

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@Today", DateTime.Today));

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    List<TrainingProgram> programs = new List<TrainingProgram>();
                    while (reader.Read())
                    {
                        TrainingProgram program = new TrainingProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                            MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees"))
                        };

                        programs.Add(program);
                    }

                    reader.Close();

                    return View(programs);
                }
            }
        }

        // GET: TrainingPrograms/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TrainingPrograms/Create
        public ActionResult Create()
        {
            TrainingProgram program = new TrainingProgram();
            return View(program);
        }

        // POST: TrainingPrograms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TrainingProgram model)
        {
            try
            {
                string sql = @"INSERT INTO TrainingProgram([Name], StartDate, EndDate, MaxAttendees)
                            VALUES(@name, @start, @end, @max)";
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.Parameters.Add(new SqlParameter("@name", model.Name));
                        cmd.Parameters.Add(new SqlParameter("@start", model.StartDate));
                        cmd.Parameters.Add(new SqlParameter("@end", model.EndDate));
                        cmd.Parameters.Add(new SqlParameter("@max", model.MaxAttendees));

                        await cmd.ExecuteNonQueryAsync();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: TrainingPrograms/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TrainingPrograms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TrainingPrograms/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TrainingPrograms/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}