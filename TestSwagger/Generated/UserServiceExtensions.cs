// Code generated by Microsoft (R) AutoRest Code Generator 0.12.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace UserServiceClient
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Rest;
    using Models;

    public static partial class UserServiceExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            public static User Get(this IUserService operations, int? id)
            {
                return Task.Factory.StartNew(s => ((IUserService)s).GetAsync(id), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<User> GetAsync( this IUserService operations, int? id, CancellationToken cancellationToken = default(CancellationToken))
            {
                HttpOperationResponse<User> result = await operations.GetWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false);
                return result.Body;
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='user'>
            /// </param>
            public static bool? Post(this IUserService operations, User user)
            {
                return Task.Factory.StartNew(s => ((IUserService)s).PostAsync(user), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='user'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<bool?> PostAsync( this IUserService operations, User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                HttpOperationResponse<bool?> result = await operations.PostWithHttpMessagesAsync(user, null, cancellationToken).ConfigureAwait(false);
                return result.Body;
            }

    }
}
