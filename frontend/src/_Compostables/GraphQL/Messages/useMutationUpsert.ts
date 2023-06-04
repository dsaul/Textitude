import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';
import type { ISMSMessage } from './ISMSMessage';

const useMutationUpsert = () => {
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)
	
	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`

	mutation e164Post($payload: Input_E164Row!) {
			settings {
				e164 {
					upsert(e164: $payload) {
						status
					}
				}
			}
		}
	`);
	
	onError((error: ApolloError) => {
		console.error('useMutationUpsert error', error);
	});
	
	const fn = (payload: ISMSMessage) => {
		mutate({
			payload: payload,
		});
	};
	
	return {
		upsert: fn,
		upsertLoading: loading,
		upsertError: onError,
		upsertDone: onDone,
	}
}

export { useMutationUpsert }