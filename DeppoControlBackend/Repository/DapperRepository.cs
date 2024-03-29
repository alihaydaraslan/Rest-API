﻿using Dapper;
using DeppoControlBackend.Result;
using System.Data;
using System.Data.SqlClient;
using IResult = DeppoControlBackend.Result.IResult;

namespace DeppoControlBackend.Repository
{
    public class DapperRepository<T> : IDapperRepository<T>
    where T : class, new()
    {
        private readonly IDbConnection _context;
        public DapperRepository()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            _context = new SqlConnection()
            {
                ConnectionString = configuration.GetConnectionString("mssqlConnection")
            };
            _context.Open();
        }

        public IDataResult<IEnumerable<T>> Get(string query, DynamicParameters p)
        {
            try
            {
                return new SuccessDataResult<IEnumerable<T>>(_context.Query<T>(query, p));
            }
            catch (Exception e)
            {
                return new ErrorDataResult<IEnumerable<T>>(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public async Task<IDataResult<IEnumerable<T>>> GetAsync(string query, DynamicParameters p)
        {
            try
            {
                return new ErrorDataResult<IEnumerable<T>>(await _context.QueryAsync<T>(query, p));
            }
            catch (Exception e)
            {
                return new ErrorDataResult<IEnumerable<T>>(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public IDataResult<T> GetById(string query, DynamicParameters p)
        {
            try
            {
                return new SuccessDataResult<T>(_context.QueryFirst<T>(query, p));
            }
            catch (Exception e)
            {
                return new ErrorDataResult<T>(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public async Task<IDataResult<T>> GetByIdAsync(string query, DynamicParameters p)
        {
            try
            {
                return new SuccessDataResult<T>(await _context.QueryFirstAsync<T>(query, p));
            }
            catch (Exception e)
            {
                return new ErrorDataResult<T>(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public IDataResult<IEnumerable<T>> GetAll(string query)
        {
            try
            {
                return new SuccessDataResult<IEnumerable<T>>(_context.Query<T>(query));
            }
            catch (Exception e)
            {
                return new ErrorDataResult<IEnumerable<T>>(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public async Task<IDataResult<IEnumerable<T>>> GetAllAsync(string query)
        {
            try
            {
                return new SuccessDataResult<IEnumerable<T>>(await _context.QueryAsync<T>(query, commandTimeout: 60));
            }
            catch (Exception e)
            {
                return new ErrorDataResult<IEnumerable<T>>(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public IDataResult<T> FirstOrDefault(string query, DynamicParameters p)
        {
            try
            {
                return new SuccessDataResult<T>(_context.QueryFirstOrDefault<T>(query, p));
            }
            catch (Exception e)
            {
                return new ErrorDataResult<T>(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public async Task<IDataResult<T>> FirstOrDefaultAsync(string query, DynamicParameters p)
        {
            try
            {
                return new SuccessDataResult<T>(await _context.QueryFirstOrDefaultAsync<T>(query, p));
            }
            catch (Exception e)
            {
                return new ErrorDataResult<T>(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public IResult ExecuteQuery(string query)
        {
            try
            {
                using IDbTransaction tran = _context.BeginTransaction();
                _context.Execute(query, transaction: tran);
                tran.Commit();
                return new SuccessResult();
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public async Task<IResult> ExecuteQueryAsync(string query)
        {
            try
            {
                IDbTransaction tran = _context.BeginTransaction();
                await _context.ExecuteAsync(query, transaction: tran);
                tran.Commit();
                return new SuccessResult();
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public IResult ExecuteQuery(string query, DynamicParameters p)
        {
            try
            {
                IDbTransaction tran = _context.BeginTransaction();
                _context.Execute(query, p, transaction: tran);
                tran.Commit();
                return new SuccessResult();
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public async Task<IResult> ExecuteQueryAsync(string query, DynamicParameters p)
        {
            try
            {
                IDbTransaction tran = _context.BeginTransaction();
                await _context.ExecuteAsync(query, p, transaction: tran);
                tran.Commit();
                return new SuccessResult();
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public IResult ExecuteProcedure(string query)
        {
            try
            {
                IDbTransaction tran = _context.BeginTransaction();
                _context.Execute(query, transaction: tran, commandType: CommandType.StoredProcedure);
                tran.Commit();
                return new SuccessResult();
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public async Task<IResult> ExecuteProcedureAsync(string query)
        {
            try
            {
                IDbTransaction tran = _context.BeginTransaction();
                await _context.ExecuteAsync(query, transaction: tran, commandType: CommandType.StoredProcedure);
                tran.Commit();
                return new SuccessResult();
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public IResult ExecuteProcedure(string query, DynamicParameters p)
        {
            try
            {
                IDbTransaction tran = _context.BeginTransaction();
                _context.Execute(query, p, transaction: tran, commandType: CommandType.StoredProcedure);
                tran.Commit();
                return new SuccessResult();
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public async Task<IResult> ExecuteProcedureAsync(string query, DynamicParameters p)
        {
            try
            {
                IDbTransaction tran = _context.BeginTransaction();
                await _context.ExecuteAsync(query, p, transaction: tran, commandType: CommandType.StoredProcedure);
                tran.Commit();
                return new SuccessResult();
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public IDataResult<IEnumerable<T>> GetWithProcedure(string query, DynamicParameters p)
        {
            try
            {
                return new SuccessDataResult<IEnumerable<T>>(_context.Query<T>(query, param: p,
                    commandType: CommandType.StoredProcedure));
            }
            catch (Exception e)
            {
                return new ErrorDataResult<IEnumerable<T>>(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public async Task<IDataResult<IEnumerable<T>>> GetWithProcedureAsync(string query, DynamicParameters p)
        {
            try
            {
                return new SuccessDataResult<IEnumerable<T>>(await _context.QueryAsync<T>(query, p, commandType: CommandType.StoredProcedure));
            }
            catch (Exception e)
            {
                return new ErrorDataResult<IEnumerable<T>>(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }

        public async Task<object> ExecuteScalarAsync(string query)
        {
            try
            {
                return await _context.ExecuteAsync(query);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
            finally
            {
                _context.Close();
                _context.Dispose();
            }
        }
    }
}
