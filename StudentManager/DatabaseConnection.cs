using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;




namespace StudentManager
{
    public class DatabaseConnection
    {
        private const string BAS_StudentsDB = "BAS_Students"; // connection name for the BAS database - see App.config
        private string BAS_StudentsDB_ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
        private SqlConnection sconn = null;
        private SqlCommand scmd = new SqlCommand();
        private SqlDataAdapter databaseAdapter = new SqlDataAdapter();
        internal SqlDataAdapter techInterestsAdapter = new SqlDataAdapter();
        private SqlDataAdapter employmentStatusAdapter = new SqlDataAdapter();

        // constructor
        public DatabaseConnection()
        {
            sconn = new SqlConnection();
            sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
        }
        // private methods
        /// <summary>
        /// Used to add a null value to the database if the string is empty. Best when called inside AddWithValue("something", DbNullIfNullOrEmpty(testString))
        /// </summary>
        /// <param name="str">string to test empty or null</param>
        /// <returns>Either the string as an object or DBNull as an object.</returns>
        private object DbNullIfNullOrEmpty(string str)
        {
            return !String.IsNullOrEmpty(str) ? (object)str : DBNull.Value;
        }

        // public methods
        public bool AttendedCollege_PrimaryKeyExists(int studentID, int collegeID)
        {
            try
            {
                using (sconn)
                using (SqlCommand scmd = new SqlCommand("SELECT CollegeID, StudentID FROM AttendedColleges WHERE CollegeID = @CollegeID AND StudentID = @StudentID"))
                {
                    scmd.CommandType = CommandType.Text;
                    scmd.Connection = sconn;

                    scmd.Parameters.AddWithValue("@StudentID", studentID);
                    scmd.Parameters.AddWithValue("@CollegeID", collegeID);

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                        sconn.Open();
                    }

                    SqlDataReader rdr = scmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Close();
                        return true;
                    }
                    return false;
                }                
            }
            catch (SqlException)
            {                
                throw;
            }
        }

        public dsStudentManager GetStudentsEmploymentStatus(int studentID)
        {
            try
            {
                // set up dataset
                dsStudentManager datEmplyomentStatus = new dsStudentManager();
                

                if (sconn.ConnectionString != BAS_StudentsDB_ConnectionString)
                {
                    sconn.ConnectionString = BAS_StudentsDB_ConnectionString;
                }

                SqlCommand selCmd = new SqlCommand();
                selCmd.Parameters.AddWithValue("@StudentID", studentID);
                selCmd.Connection = sconn;
                selCmd.CommandType = CommandType.Text;
                selCmd.CommandText = "SELECT EmploymentStatus.EmploymentStatusID, EmployerName, EmploymentStatus, StatusChangedDate, JobTitle FROM Employers INNER JOIN EmploymentStatus ON Employers.EmployerID = EmploymentStatus.EmployerID INNER JOIN EmploymentStatusLookUp ON EmploymentStatus.Status = EmploymentStatusLookUp.EmploymentStatusID INNER JOIN JobTitleLookUp ON EmploymentStatus.JobTitleID = JobTitleLookUp.JobTitleID WHERE StudentID = @StudentID";

                // set properties of adapter
                employmentStatusAdapter.SelectCommand = selCmd;

                // fill authors table of this dataset
                employmentStatusAdapter.Fill(datEmplyomentStatus);

                return datEmplyomentStatus;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sconn.Close();
            }
        }
        public dsStudentManager GetStudentsPreviousColleges(int studentID)
        {
            try
            {
                // set up dataset
                dsStudentManager datPreviousColleges = new dsStudentManager();

                // set up command objects
                scmd.Parameters.Clear();
                scmd.Parameters.AddWithValue("@StudentID", studentID);

                if (sconn.ConnectionString != BAS_StudentsDB_ConnectionString)
                {
                    sconn.ConnectionString = BAS_StudentsDB_ConnectionString;
                }

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.Text;
                scmd.CommandText = "SELECT Collegename, CurrentlyAttending, LastYearAttended, GPA FROM Colleges INNER JOIN AttendedColleges ON Colleges.CollegeID = AttendedColleges.CollegeID WHERE StudentID = @StudentID ORDER BY LastYearAttended DESC";

                // set properties of adapter
                databaseAdapter.SelectCommand = scmd;

                // fill authors table of this dataset
                databaseAdapter.Fill(datPreviousColleges);

                // can not use command builder to auto generate Insert Update and Delete commands.
                //SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(dbAdapter);

                return datPreviousColleges;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sconn.Close();
            }
        }
        public dsStudentManager GetStudentsTechInterests(int studentID)
        {
            try
            {
                // set up dataset
                dsStudentManager datTechInterests = new dsStudentManager();
                // set up command objects
                scmd.Parameters.Clear();
                scmd.Parameters.AddWithValue("@StudentID", studentID);

                if (sconn.ConnectionString != BAS_StudentsDB_ConnectionString)
                {
                    sconn.ConnectionString = BAS_StudentsDB_ConnectionString;
                }

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.Text;
                scmd.CommandText = "SELECT * FROM TechInterests WHERE StudentID = @StudentID";                

                // set properties of adapter
                techInterestsAdapter.SelectCommand = scmd;

                // fill authors table of this dataset
                techInterestsAdapter.Fill(datTechInterests, "TechInterests");

                // use command builder to auto generate Insert Update and Delete commands.
                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(techInterestsAdapter);

                return datTechInterests;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sconn.Close();
            }
        }
        public dsStudentManager GetAllStudentsInDataSet() 
        {
            try 
            {
                // set up dataset
                dsStudentManager datAllStudents = new dsStudentManager();

                // set up command objects
                scmd.Parameters.Clear();
                scmd.Connection = sconn;
                scmd.CommandType = CommandType.Text;
                scmd.CommandText = "SELECT * FROM Students";

                SqlCommandBuilder scb = new SqlCommandBuilder(databaseAdapter);

                // set properties of adapter
                databaseAdapter.SelectCommand = scmd;
                databaseAdapter.UpdateCommand = scb.GetUpdateCommand();
                databaseAdapter.InsertCommand = scb.GetInsertCommand();

                // fill dataset with needed tables
                databaseAdapter.Fill(datAllStudents, "Students");
                scmd.CommandText = "SELECT * FROM Courses";
                databaseAdapter.Fill(datAllStudents, "Courses");
                scmd.CommandText = "SELECT * FROM CourseEnrollment";
                databaseAdapter.Fill(datAllStudents, "CourseEnrollment");
                scmd.CommandText = "SELECT * FROM StudentDetails";
                databaseAdapter.Fill(datAllStudents, "StudentDetails");

                // can not use command builder to auto generate Insert Update and Delete commands for views.
                //SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(dbAdapter);

                return datAllStudents;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sconn.Close();
            }
        }

        public SqlDataReader GetEmploymentStatusByID(int employmentStatusID)
        {
            try
            {
                SqlCommand scom = new SqlCommand("SELECT * FROM EmploymentStatus WHERE EmploymentStatusID = @EmploymentStatusID");
                
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }
                scom.Connection = sconn;
                scom.CommandType = CommandType.Text;

                scom.Parameters.AddWithValue("@EmploymentStatusID", employmentStatusID);

                return scom.ExecuteReader(CommandBehavior.CloseConnection);
                
            }
            catch (SqlException)
            {                
                throw;
            }
        }
        public SqlDataReader StudentsWhoHaveCompletedClass(int courseID)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }
                scmd.Parameters.Clear();

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = "StudentsWhoHaveCompletedClass"; // stored procedure name

                scmd.Parameters.AddWithValue("@courseID", courseID);

                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader GetStudentByID(int studentID)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }
                scmd.Parameters.Clear();

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = "GetStudentByID"; // stored procedure name

                scmd.Parameters.AddWithValue("@studentID", studentID);

                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader StudentsWhoNeedToCompleteClass(int courseID)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }
                scmd.Parameters.Clear();

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = "StudentsWhoNeedToCompleteClass"; // stored procedure name

                scmd.Parameters.AddWithValue("@courseID", courseID);

                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader StudentsWithPreReqsLeft()
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Parameters.Clear();

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = "StudentsWithPreReqsLeft"; // stored procedure name

                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader ReturnBASGraduates(int yrStart, int yrEnd)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Parameters.Clear();
                scmd.Parameters.AddWithValue("@YearStart", yrStart);
                scmd.Parameters.AddWithValue("@YearEnd", yrEnd);

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = "GetBASGraduates"; // stored procedure name

                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader ReturnRowsFromStoredProcedure(string sProcName)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Parameters.Clear();

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = sProcName; // stored procedure name

                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader ReturnRowsFromStoredProcedure(string sProcName, Dictionary<string, object> procParameters)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Parameters.Clear();
                if (procParameters.Count > 0)
                {
                    foreach (var param in procParameters)
                    {
                        scmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = sProcName; // stored procedure name

                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader GetMostRecentStudentDetails(int studentID)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Parameters.Clear();

                scmd.Parameters.AddWithValue("@studentID", studentID);

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = "GetLastUpdatedStudentDetails"; // stored procedure name

                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader GetStudentEnrollmentData(int studentID)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Parameters.Clear();
                scmd.Parameters.AddWithValue("@StudentID", studentID);
                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = "GetStudentEnrollmentData"; // stored procedure name

                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
        public SqlDataReader GetAllRowsFromTable(string table)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Parameters.Clear();

                scmd.Connection = sconn;
                scmd.CommandText = "SELECT * FROM " + table;
                scmd.CommandType = CommandType.Text;


                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader GetCoursesCreditSection()
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Parameters.Clear();

                scmd.Connection = sconn;
                scmd.CommandText = "SELECT CourseID, CourseNumber, CourseName FROM Courses ORDER BY CreditSection" ;
                scmd.CommandType = CommandType.Text;


                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Gets the total number of students in the database
        /// </summary>
        /// <returns>An Integer indicating the number of students in the database</returns>
        public int GetTotalStudents()
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("SELECT Count(StudentID) FROM Students"))
                {

                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;


                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                        sconn.Open();
                    }

                    return (int)sqlComm.ExecuteScalar();
                }
            }
            catch (SqlException sqx)
            {
                throw sqx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetLastStudent(int studentID, out string firstName, out string lastName)
        {
            try
            {
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.Text;
                if (studentID == 0)
                {
                    scmd.CommandText = "SELECT StudentID, FirstName, LastName FROM Students WHERE StudentID = (SELECT MAX(StudentID) FROM Students)";
                }
                else
                    scmd.CommandText = "SELECT StudentID, FirstName, LastName FROM Students WHERE StudentID = " + studentID.ToString();


                SqlDataReader sdr = scmd.ExecuteReader();

                firstName = "";
                lastName = "";
                if (sdr.Read())
                {
                    firstName = (string)sdr["FirstName"];
                    lastName = (string)sdr["LastName"];
                    // get studentID again
                    int i = sdr.GetInt32(0);
                    sdr.Close();
                    return i;
                }

                return -1;

            }
            catch (SqlException sqx)
            {
                throw sqx;
            }
            finally
            {
                sconn.Close();
            }
        }        
        public SqlDataReader GetAllCourses()
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Parameters.Clear();

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.Text;
                scmd.CommandText = "SELECT c.CourseID, c.CourseNumber, c.CourseItemNumber, csl.CreditSection, c.CourseName, c.CourseDescription, c.Credits, c.RequiredForBASAdmission, c.RequiredForBASCompletion FROM Courses c INNER JOIN CreditSectionLookup csl ON c.CreditSection = csl.CreditSectionID";

                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader GetAllCoursesRequiredForBASAdmission(bool andCompletion)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Parameters.Clear();

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.Text;
                if (andCompletion)
                {
                    scmd.CommandText = "SELECT * FROM Courses WHERE RequiredForBASAdmission = 1 AND RequiredForBASCompletion = 1";
                }
                else
                {
                    scmd.CommandText = "SELECT * FROM Courses WHERE RequiredForBASAdmission = 1";
                }
                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader GetAllCoursesRequiredForBASAdmission()
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Parameters.Clear();

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.Text;

                scmd.CommandText = "SELECT * FROM Courses WHERE RequiredForBASAdmission = 1";


                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader GetCourseData(int courseID)
        {            
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Parameters.Clear();

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.Text;

                scmd.CommandText = "SELECT * FROM Courses WHERE CourseID = " + courseID.ToString();


                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader GetAllStudents(string orderBy, bool descending)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                // set the connection properties for the command object
                scmd.Connection = sconn;

                // command type:
                scmd.CommandType = CommandType.Text;
                if (descending)
                    scmd.CommandText = "SELECT * FROM Students ORDER BY " + orderBy + " DESC";
                else
                    scmd.CommandText = "SELECT * FROM Students ORDER BY " + orderBy;

                // execute the methods

                // command behavior closeconnection will close the connection when the datareader is 
                // closed in the interface.
                // Do not close the connection in the finally block.
                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        // public methods cont. --- These methods change or update the database
        public int RemoveCourse(int courseID)
        {            
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Parameters.Clear();

                scmd.Parameters.AddWithValue("@courseID", courseID);
                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = "DeleteCourseWithID"; // stored procedure name

                return scmd.ExecuteNonQuery();
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sconn.Close();
            }
        }
        public bool AddAttendedCollege(int collegeID, int studentID, bool currentlyAttending, int lastYearAttended, double gpa)
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("INSERT INTO AttendedColleges VALUES(@CollegeID, @StudentID, @CurrentlyAttending, @LastYearAttended, @GPA)"))
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;

                    sqlComm.Parameters.AddWithValue("@CollegeID", collegeID);
                    sqlComm.Parameters.AddWithValue("@StudentID", studentID);
                    sqlComm.Parameters.AddWithValue("@CurrentlyAttending", currentlyAttending);
                    sqlComm.Parameters.AddWithValue("@LastYearAttended", lastYearAttended);
                    sqlComm.Parameters.AddWithValue("@GPA", gpa);

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                        sconn.Open();
                    }

                    sqlComm.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateAttendedCollege(int collegeID, int studentID, bool currentlyAttending, int lastYearAttended, double gpa)
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("UPDATE AttendedColleges SET CurrentlyAttending = @CurrentlyAttending, LastYearAttended = @LastYearAttended, GPA = @GPA WHERE CollegeID = @CollegeID AND StudentID = @StudentID"))
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;

                    sqlComm.Parameters.AddWithValue("@CollegeID", collegeID);
                    sqlComm.Parameters.AddWithValue("@StudentID", studentID);
                    sqlComm.Parameters.AddWithValue("@CurrentlyAttending", currentlyAttending);
                    sqlComm.Parameters.AddWithValue("@LastYearAttended", lastYearAttended);
                    sqlComm.Parameters.AddWithValue("@GPA", gpa);

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                        sconn.Open();
                    }

                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool AddCollege(string collegeName)
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("INSERT INTO Colleges VALUES(@CollegeName)"))
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;

                    sqlComm.Parameters.AddWithValue("@CollegeName", collegeName);

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = BAS_StudentsDB_ConnectionString;
                        sconn.Open();
                    }

                    sqlComm.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool UpdateStudent(int studentID, string rtcID, string fName, string lName, string phoneNo, string sEmail, string primaryEmail, string address, string city, string state, string zip, DateTime birthDate, bool gender, string notes, string documentsPath)
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("UPDATE Students SET RTCStudentID = @RTCStudentID, FirstName = @FirstName, LastName = @LastName, Phone = @Phone, StudentEmail = @StudentEmail, PersonalEmail = @PersonalEmail, Address = @Address, City = @City, State = @State, Zip = @Zip, BirthDate = @BirthDate, Gender = @Gender, Notes = @Notes, StudentDocumentsLocation = @StudentDocumentsLocation WHERE StudentID = @StudentID"))
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;
                    sqlComm.Parameters.AddWithValue("@RTCStudentID", DbNullIfNullOrEmpty(rtcID));
                    sqlComm.Parameters.AddWithValue("@FirstName", fName);
                    sqlComm.Parameters.AddWithValue("@LastName", lName);
                    sqlComm.Parameters.AddWithValue("@Phone", DbNullIfNullOrEmpty(phoneNo));
                    sqlComm.Parameters.AddWithValue("@StudentEmail", DbNullIfNullOrEmpty(sEmail));
                    sqlComm.Parameters.AddWithValue("@PersonalEmail", primaryEmail);
                    sqlComm.Parameters.AddWithValue("@Address", DbNullIfNullOrEmpty(address));
                    sqlComm.Parameters.AddWithValue("@City", DbNullIfNullOrEmpty(city));
                    sqlComm.Parameters.AddWithValue("@State", DbNullIfNullOrEmpty(state));
                    if (zip == "0")
                        sqlComm.Parameters.AddWithValue("@Zip", DBNull.Value);
                    else
                        sqlComm.Parameters.AddWithValue("@Zip", zip);
                    
                    if (birthDate == DateTime.Now.Date)
                        sqlComm.Parameters.AddWithValue("@BirthDate", DBNull.Value);
                    else
                        sqlComm.Parameters.AddWithValue("@BirthDate", birthDate);
                    
                    sqlComm.Parameters.AddWithValue("@Gender", gender);
                    sqlComm.Parameters.AddWithValue("@Notes", DbNullIfNullOrEmpty(notes));
                    sqlComm.Parameters.AddWithValue("@StudentDocumentsLocation", DbNullIfNullOrEmpty(documentsPath));
                    sqlComm.Parameters.AddWithValue("@StudentID", studentID);

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                        sconn.Open();
                    }

                    sqlComm.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException)
            {
                throw;
            }
        }
        /// <summary>
        /// THIS METHOD STILL IN DEVELOPMENT
        /// </summary>
        /// <param name="studentID"></param>
        /// <param name="eduBackground"></param>
        /// <param name="referralType"></param>
        /// <param name="contactMethod"></param>
        /// <param name="attendedInfoSession"></param>
        /// <param name="runningStartParticipant"></param>
        /// <param name="BASStatus"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        public bool AddStudentDetail(int studentID, int eduBackground, int referralType, int contactMethod, bool attendedInfoSession, bool runningStartParticipant, int BASStatus, string notes)
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("INSERT INTO StudentDetails VALUES(@StudentID, @EduBackground, @ReferralType, @PreferredContactMethod, @AttendedInfoSession, @RunningStartParticipant, @BAS_Status, @LastUpdated, @Notes)"))
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;

                    sqlComm.Parameters.AddWithValue("@StudentID", studentID);
                    sqlComm.Parameters.AddWithValue("@EduBackground", eduBackground);
                    sqlComm.Parameters.AddWithValue("@ReferralType", referralType);
                    sqlComm.Parameters.AddWithValue("@PreferredContactMethod", contactMethod);
                    sqlComm.Parameters.AddWithValue("@AttendedInfoSession", attendedInfoSession);
                    sqlComm.Parameters.AddWithValue("@RunningStartParticipant", runningStartParticipant);
                    sqlComm.Parameters.AddWithValue("@BAS_Status", BASStatus);
                    sqlComm.Parameters.AddWithValue("@LastUpdated", DateTime.Now);
                    sqlComm.Parameters.AddWithValue("@Notes", DbNullIfNullOrEmpty(notes));


                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                        sconn.Open();
                    }

                    sqlComm.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Used to add a student to a course
        /// </summary>
        /// <param name="studentID">Students StudentID</param>
        /// <param name="courseID">ID of the course they are being enrolled in</param>
        /// <param name="enrollmentStatus">Students enrollment status as an integer (see lookup table)</param>
        /// <param name="enrollmentDate">The date the student was enrolled in this course</param>
        /// <param name="conditionalAdmission">Whether or not the student is circumventing another prerequisite</param>
        /// <param name="notes">Additional information about this particular enrollment</param>
        /// <returns></returns>
        public bool EnrollStudent(int studentID, int courseID, int enrollmentStatus, DateTime enrollmentDate, bool conditionalAdmission, string notes)
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("INSERT INTO CourseEnrollment VALUES(@StudentID, @CourseID, @EnrollmentStatus, @EnrollmentDate, @ConditionalAdmission, @Notes)"))
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;

                    sqlComm.Parameters.AddWithValue("@StudentID", studentID);
                    sqlComm.Parameters.AddWithValue("@CourseID", courseID);
                    sqlComm.Parameters.AddWithValue("@EnrollmentStatus", enrollmentStatus);
                    sqlComm.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);
                    sqlComm.Parameters.AddWithValue("@ConditionalAdmission", conditionalAdmission);
                    sqlComm.Parameters.AddWithValue("@Notes", DbNullIfNullOrEmpty(notes));

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                        sconn.Open();
                    }

                    sqlComm.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Adds a new student to the database
        /// </summary>
        /// <param name="sid">Renton Techs Student ID number</param>
        /// <param name="fName">Students first name</param>
        /// <param name="lName">Students last name</param>
        /// <param name="phoneNo">Students phone number</param>
        /// <param name="sEmail">Students RTC email</param>
        /// <param name="primaryEmail">Students personal email</param>
        /// <param name="address">Students resident address</param>
        /// <param name="city">Students resident city</param>
        /// <param name="state">Students resident state</param>
        /// <param name="zip">Students resident zip</param>
        /// <param name="birthDate">Date the student was born</param>
        /// <param name="gender">Students gender identification</param>
        /// <param name="notes">Additional information about this student</param>
        /// <param name="documentsPath">The folder location where this student's transcrips and other documents will be saved</param>

        /// <returns></returns>
        public bool AddStudent(string sid, string fName, string lName, string phoneNo, string sEmail, string primaryEmail, string address, string city, string state, string zip, DateTime birthDate, bool gender, string notes, string documentsPath)
        {            
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("INSERT INTO Students VALUES(@RTCStudentID, @FirstName, @LastName, @Phone, @StudentEmail, @PersonalEmail, @Address, @City, @State, @Zip, @BirthDate, @Gender, @Notes, @StudentDocumentsLocation)"))
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;

                    sqlComm.Parameters.AddWithValue("@RTCStudentID", DbNullIfNullOrEmpty(sid));
                    sqlComm.Parameters.AddWithValue("@FirstName", fName);
                    sqlComm.Parameters.AddWithValue("@LastName", lName);
                    sqlComm.Parameters.AddWithValue("@Phone", DbNullIfNullOrEmpty(phoneNo));
                    sqlComm.Parameters.AddWithValue("@StudentEmail", DbNullIfNullOrEmpty(sEmail));
                    sqlComm.Parameters.AddWithValue("@PersonalEmail", primaryEmail);
                    sqlComm.Parameters.AddWithValue("@Address", DbNullIfNullOrEmpty(address));
                    sqlComm.Parameters.AddWithValue("@City", DbNullIfNullOrEmpty(city));
                    sqlComm.Parameters.AddWithValue("@State", DbNullIfNullOrEmpty(state));
                    if (zip == "0")
                        sqlComm.Parameters.AddWithValue("@Zip", DBNull.Value);
                    else
                        sqlComm.Parameters.AddWithValue("@Zip", zip);

                    if (birthDate.Date == DateTime.Now.Date)
                        sqlComm.Parameters.AddWithValue("@BirthDate", DBNull.Value);
                    else
                        sqlComm.Parameters.AddWithValue("@BirthDate", birthDate);

                    sqlComm.Parameters.AddWithValue("@Gender", gender);
                    sqlComm.Parameters.AddWithValue("@Notes", DbNullIfNullOrEmpty(notes));
                    sqlComm.Parameters.AddWithValue("@StudentDocumentsLocation", DbNullIfNullOrEmpty(documentsPath));

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                        sconn.Open();
                    }

                    sqlComm.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException sqx)
            {
                throw sqx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// UNUSED UNFINISHED
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="phoneNo"></param>
        /// <param name="sEmail"></param>
        /// <param name="primaryEmail"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <param name="birthDate"></param>
        /// <param name="gender"></param>
        /// <param name="notes"></param>
        /// <param name="documentsPath"></param>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public bool AddStudent_WithDisconnectedArchitecture(string sid, string fName, string lName, string phoneNo, string sEmail, string primaryEmail, string address, string city, string state, string zip, DateTime birthDate, bool gender, string notes, string documentsPath, dsStudentManager dataSet) 
        {
            // create a new row and set the value for each column.
            dsStudentManager.StudentsRow newRow = (dsStudentManager.StudentsRow)dataSet.Students.NewRow();
            newRow.RTCStudentID = sid;
            newRow.FirstName = fName;
            newRow.LastName = lName;
            newRow.Phone = phoneNo;
            newRow.StudentEmail = sEmail;
            newRow.PersonalEmail = primaryEmail;
            newRow.Address = address;
            newRow.State = state;
            newRow.City = city;
            newRow.Zip = zip;
            newRow.BirthDate = birthDate;
            newRow.Gender = gender;
            newRow.Notes = notes;
            newRow.StudentDocumentsLocation = documentsPath;

            // add the row to the dataset
            dataSet.Students.Rows.Add(newRow);
            


            return true;
        }
        /// <summary>
        /// Adds a course to the database. returns false if a course exists with the same Course Number or Item Number 
        /// </summary>
        /// <param name="courseNumber">The course identification number EX: CSI 101</param>
        /// <param name="itemNumber">The course identification number used by registration</param>
        /// <param name="courseName">Name of the course EX: Java for C# programmers</param>
        /// <param name="courseDescription">A short description of the course</param>
        /// <param name="credits">The number of credits recieved for satisfactory completion</param>
        /// <param name="requiredBASAdmission">Whether or not this course is required for admission to the BAS program</param>
        /// <param name="requiredBASCompletion">Whether or not this course is required for completion of the BAS program</param>
        /// <returns>Returns true if the course was added successfully</returns>
        public bool AddCourse(string courseNumber, string itemNumber, string courseName, int courseTypeID, string courseDescription, int credits, bool requiredBASAdmission, bool requiredBASCompletion, int creditSection)
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("INSERT INTO Courses VALUES(@CourseNumber, @CourseItemNumber, @CourseName, @CourseTypeID, @CourseDescription, @Credits, @RequiredForBASAdmission, @RequiredForBASCompletion, @CreditSection)"))
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;
                    sqlComm.Parameters.AddWithValue("@CourseNumber", courseNumber);
                    sqlComm.Parameters.AddWithValue("@CourseItemNumber", itemNumber);
                    sqlComm.Parameters.AddWithValue("@CourseName", courseName);
                    sqlComm.Parameters.AddWithValue("@CourseTypeID", courseTypeID);

                    if (courseDescription == string.Empty)
                    {
                        sqlComm.Parameters.AddWithValue("@CourseDescription", DBNull.Value);
                    }
                    else
                        sqlComm.Parameters.AddWithValue("@CourseDescription", courseDescription);
                    
                    sqlComm.Parameters.AddWithValue("@Credits", credits);
                    sqlComm.Parameters.AddWithValue("@RequiredForBASAdmission", requiredBASAdmission);
                    sqlComm.Parameters.AddWithValue("@RequiredForBASCompletion", requiredBASCompletion);
                    sqlComm.Parameters.AddWithValue("@CreditSection", creditSection);

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                        sconn.Open();
                    }

                    // execute stored procedure and check to see if the course number or item number already exist
                    int retRows = 0;
                    using (SqlCommand storedProcCmd = new SqlCommand("CheckForDuplicateCourseNumberOrItem"))
                    {
                        storedProcCmd.CommandType = CommandType.StoredProcedure;
                        storedProcCmd.Connection = sconn;

                        storedProcCmd.Parameters.AddWithValue("@CourseNumber", courseNumber);
                        storedProcCmd.Parameters.AddWithValue("@CourseItemNumber", itemNumber);                        
                        storedProcCmd.Parameters.AddWithValue("@RowCount", retRows).Direction = ParameterDirection.Output;

                        storedProcCmd.ExecuteNonQuery();
                        retRows = (int)storedProcCmd.Parameters["@RowCount"].Value;
                    }
                    if (retRows == 0)
                    {
                        sqlComm.ExecuteNonQuery();
                        return true;
                    }                    
                }
                return false;
            }
            catch (SqlException sqx)
            {
                throw sqx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Modifies the course in the database that matches the provided course ID.
        /// </summary>
        /// <param name="courseID">Uniqe ID used to differentiate courses</param>
        /// <param name="CourseNumber">Course Number as appears in Course Catalogs. Ex: CSI 100</param>
        /// <param name="itemNumber">Number used by registration to differentiate courses</param>
        /// <param name="courseName">Name of the course</param>
        /// <param name="courseTypeID">ID number determining which type of course this is</param>
        /// <param name="courseDescription">Short description about the course</param>
        /// <param name="credits">Number of credits the course is worth</param>
        /// <param name="requiredBASAdmission">Whether or not this course is required for admission to the BAS program</param>
        /// <param name="requiredBASCompletion">Whether or not this course is required for completion of the BAS program</param>
        /// <param name="creditSection">An integer to determine which section this course provides credit for</param>
        /// <returns></returns>
        public bool ModifyCourse(int courseID, string courseNumber, string itemNumber, string courseName, int courseTypeID, string courseDescription, int credits, bool requiredBASAdmission, bool requiredBASCompletion, int creditSection)
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("UPDATE Courses SET CourseNumber = @CourseNumber, CourseItemNumber = @CourseItemNumber, CourseName = @CourseName, CourseDescription = @CourseDescription, Credits = @Credits, RequiredForBASAdmission = @RequiredForBASAdmission, RequiredForBASCompletion = @RequiredForBASCompletion WHERE CourseID = @CourseID"))
                {

                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;
                    sqlComm.Parameters.AddWithValue("@CourseID", courseID);
                    sqlComm.Parameters.AddWithValue("@CourseNumber", courseNumber);
                    sqlComm.Parameters.AddWithValue("@CourseItemNumber", itemNumber);
                    sqlComm.Parameters.AddWithValue("@CourseName", courseName);
                    sqlComm.Parameters.AddWithValue("@CourseTypeID", courseTypeID);
                    sqlComm.Parameters.AddWithValue("@CourseDescription", DbNullIfNullOrEmpty(courseDescription));
                    sqlComm.Parameters.AddWithValue("@Credits", credits);
                    sqlComm.Parameters.AddWithValue("@RequiredForBASAdmission", requiredBASAdmission);
                    sqlComm.Parameters.AddWithValue("@RequiredForBASCompletion", requiredBASCompletion);
                    sqlComm.Parameters.AddWithValue("@CreditSection", creditSection);

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                        sconn.Open();
                    }

                    // execute stored procedure and check to see if the course number or item number already exist
                    int retRows = 0;
                    using (SqlCommand storedProcCmd = new SqlCommand("CheckForDuplicateCourseNumberOrItem"))
                    {
                        storedProcCmd.CommandType = CommandType.StoredProcedure;
                        storedProcCmd.Connection = sconn;

                        storedProcCmd.Parameters.AddWithValue("@CourseNumber", courseNumber);
                        storedProcCmd.Parameters.AddWithValue("@CourseItemNumber", itemNumber);
                        storedProcCmd.Parameters.AddWithValue("@RowCount", retRows).Direction = ParameterDirection.Output;

                        storedProcCmd.ExecuteNonQuery();
                        retRows = (int)storedProcCmd.Parameters["@RowCount"].Value;
                    }
                    if (retRows == 0)
                    {
                        sqlComm.ExecuteNonQuery();
                        return true;
                    }  
                }
                return false;
            }
            catch (SqlException sqx)
            {
                throw sqx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public bool RemoveCollege(int collegeID)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = BAS_StudentsDB_ConnectionString;
                    sconn.Open();
                }

                scmd.Parameters.Clear();

                scmd.Parameters.AddWithValue("@CollegeID", collegeID);
                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = "DeleteCollegeWithID"; // stored procedure name

                int retval = scmd.ExecuteNonQuery();
                if (retval > 0)
                    return true;
                else
                    return false;

            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sconn.Close();
            }
        }
        public void AddEmployer(string employerName)
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("INSERT INTO Employers VALUES(@EmployerName)"))
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;

                    sqlComm.Parameters.AddWithValue("@EmployerName", employerName);

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = BAS_StudentsDB_ConnectionString;
                        sconn.Open();
                    }

                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool RemoveEmployer(int employerID)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = BAS_StudentsDB_ConnectionString;
                    sconn.Open();
                }

                scmd.Parameters.Clear();

                scmd.Parameters.AddWithValue("@EmployerID", employerID);
                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = "DeleteEmployerWithID"; // stored procedure name

                int retval = scmd.ExecuteNonQuery();
                if (retval > 0)
                    return true;
                else
                    return false;

            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sconn.Close();
            }
        }
        public void AddEmploymentStatus(int selectedEmployerID, int studentID, int employmentStatus, int jobTitle, DateTime dateTime)
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("INSERT INTO EmploymentStatus VALUES(@EmployerID, @StudentID, @Status, @JobTitleID, @StatusChangedDate)"))
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;

                    sqlComm.Parameters.AddWithValue("@EmployerID", selectedEmployerID);
                    sqlComm.Parameters.AddWithValue("@StudentID", studentID);
                    sqlComm.Parameters.AddWithValue("@Status", employmentStatus);
                    sqlComm.Parameters.AddWithValue("@JobTitleID", jobTitle);
                    sqlComm.Parameters.AddWithValue("@StatusChangedDate", dateTime);

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                        sconn.Open();
                    }

                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteEmploymentStatus(int StatusID)
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("DELETE FROM EmploymentStatus WHERE EmploymentStatusID = @EmploymentStatusID"))
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;

                    sqlComm.Parameters.AddWithValue("@EmploymentStatusID", StatusID);

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                        sconn.Open();
                    }

                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AddJobTitle(string jobTitle)
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("INSERT INTO JobTitleLookUp VALUES(@JobTitle)"))
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;

                    sqlComm.Parameters.AddWithValue("@JobTitle", jobTitle);

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = BAS_StudentsDB_ConnectionString;
                        sconn.Open();
                    }

                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool RemoveJobTitle(int jobTitleID)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = BAS_StudentsDB_ConnectionString;
                    sconn.Open();
                }

                scmd.Parameters.Clear();

                scmd.Parameters.AddWithValue("@JobTitleID", jobTitleID);
                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = "DeleteJobTitleWithID"; // stored procedure name

                int retval = scmd.ExecuteNonQuery();
                if (retval > 0)
                    return true;
                else
                    return false;

            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sconn.Close();
            }
        }
        public void AddCourseEnrollmentData(int studentID, int courseID, int enrollmentStatus, DateTime enrollmentDate, DateTime? completionDate, bool conditionalAdmission, bool equivalentCompletion, string notes)
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("INSERT INTO CourseEnrollment VALUES(@StudentID, @CourseID, @EnrollmentStatus, @EnrollmentDate, @CompletionDate, @ConditionalAdmission, @TransferEquivalent, @Notes)"))
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;

                    sqlComm.Parameters.AddWithValue("@StudentID", studentID);
                    sqlComm.Parameters.AddWithValue("@CourseID", courseID);
                    sqlComm.Parameters.AddWithValue("@EnrollmentStatus", enrollmentStatus);
                    sqlComm.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);
                    if (completionDate == null)
                        sqlComm.Parameters.AddWithValue("@CompletionDate", DBNull.Value);
                    else
                        sqlComm.Parameters.AddWithValue("@CompletionDate", completionDate);
                    sqlComm.Parameters.AddWithValue("@ConditionalAdmission", conditionalAdmission);
                    sqlComm.Parameters.AddWithValue("@TransferEquivalent", equivalentCompletion);
                    sqlComm.Parameters.AddWithValue("@Notes", DbNullIfNullOrEmpty(notes));

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                        sconn.Open();
                    }

                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateCourseEnrollmentData(int studentID, int enrollmentID, int enrollmentStatus, DateTime enrollmentDate, DateTime? completionDate, bool conditionalAdmission, bool equivalentCompletion, string notes)
        {
            try
            {
                using (sconn)
                using (SqlCommand sqlComm = new SqlCommand("UPDATE CourseEnrollment SET StudentID = @StudentID, EnrollmentStatus = @EnrollmentStatus, EnrollmentDate = @EnrollmentDate, CompletionDate = @CompletionDate, ConditionalAdmission = @ConditionalAdmission, TransferEquivalent = @TransferEquivalent, Notes = @Notes WHERE EnrollmentID = @EnrollmentID"))
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = sconn;

                    sqlComm.Parameters.AddWithValue("@StudentID", studentID);
                    sqlComm.Parameters.AddWithValue("@EnrollmentStatus", enrollmentStatus);
                    sqlComm.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);
                    if (completionDate == null)
                        sqlComm.Parameters.AddWithValue("@CompletionDate", DBNull.Value);
                    else
                        sqlComm.Parameters.AddWithValue("@CompletionDate", completionDate);
                    sqlComm.Parameters.AddWithValue("@ConditionalAdmission", conditionalAdmission);
                    sqlComm.Parameters.AddWithValue("@TransferEquivalent", equivalentCompletion);
                    sqlComm.Parameters.AddWithValue("@Notes", DbNullIfNullOrEmpty(notes));
                    sqlComm.Parameters.AddWithValue("@EnrollmentID", enrollmentID);

                    if (sconn.State != ConnectionState.Open)
                    {
                        sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                        sconn.Open();
                    }

                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SqlDataReader GetStudentDetails(int studentID)
        {
            try
            {
                // open the connection
                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = "GetStudentDetails";

                return scmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int TimesCompletedOrEnrolled(int studentID, int courseID)
        {
            using (sconn)
            using (scmd)
            {
                scmd.Connection = sconn;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = "TimesEnrolledInOrCompletedCourse";

                scmd.Parameters.Clear();
                scmd.Parameters.AddWithValue("@StudentID", studentID);
                scmd.Parameters.AddWithValue("@CourseID", courseID);

                if (sconn.State != ConnectionState.Open)
                {
                    sconn.ConnectionString = ConfigurationManager.ConnectionStrings[BAS_StudentsDB].ToString();
                    sconn.Open();
                }

                return (int)scmd.ExecuteScalar();
            }
        }


    }
}
